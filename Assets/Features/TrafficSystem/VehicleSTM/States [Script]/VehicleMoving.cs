using System.Collections;
using UnityEngine;

[CreateAssetMenu(fileName = "VehicleMoving", menuName = "TrafficSystem/States/VehicleMoving")]
public class VehicleMoving : BaseState<VehicleSTM>
{
    #region variables
    private Vector3 startingMapPoint;
    private Vector3 targetMapPoint;
    #endregion

    public override void OnEnter(VehicleSTM stm)
    {
        CheckTransition(stm);
        stm.StartCoroutine(Accelerate(stm));
    }

    public override void OnUpdate(VehicleSTM stm)
    {
        stm.transform.position += stm.ActivePath.Direction * Time.deltaTime * stm.CurrentSpeed;
        CheckTransition(stm);
    }

    public override void OnExit(VehicleSTM stm)
    {
        //nothing
    }

    public IEnumerator Accelerate(VehicleSTM stm) {
        float startingSpeed = stm.CurrentSpeed;
        float accelerationTime = stm.AccelerationTime * (stm.TopSpeed - startingSpeed) / stm.TopSpeed;

        float elapsedTime = 0;
        while (stm.CurrentSpeed < stm.TopSpeed) {
            elapsedTime += Time.fixedDeltaTime;
            stm.CurrentSpeed = Mathf.Lerp(startingSpeed, stm.TopSpeed, elapsedTime / accelerationTime);
            yield return new WaitForFixedUpdate();
        }
    }

    private void CheckTransition(VehicleSTM stm) {
        
        Vector3 raycastOrigin = stm.VehicleBound.bounds.center + stm.VehicleBound.bounds.extents.x * stm.transform.forward;
        Vector3 raycastDirection = stm.transform.forward; 

        #region transition if there is vehicle in front
        
        // check vehicle in front
        LayerMask intersection = LayerMask.GetMask(new string[]{"TrafficIntersection"});
        LayerMask vehicle = LayerMask.GetMask(new string[]{"Vehicle"});
        LayerMask allObstacle = LayerMask.GetMask(new string[]{"TrafficIntersection", "Vehicle"});

        RaycastHit hitInfo;
        bool vehicleAhead = Physics.Raycast(raycastOrigin, raycastDirection, out hitInfo, stm.FrontDetectionRange, vehicle);
        if (vehicleAhead) {
            // check if lane switch is possible
            
            VehicleSTM vehicleHit = hitInfo.transform.parent.GetComponent<VehicleSTM>();

            if (!vehicleHit) {
                Debug.LogWarning("Vehicle hit is not a VehicleSTM");
                return;
            }

            if (vehicleHit.CurrentSpeed > stm.CurrentSpeed) {
                stm.StopAllCoroutines();
                stm.ChangeState("VehicleStopping");
                return;
            }

            // check left lane
            bool leftLaneAvailable = stm.LaneNumber < stm.ActivePath.NumberOfLanes - 1;
            if (leftLaneAvailable) {
                Vector3 leftRaycastOrigin = raycastOrigin + stm.transform.right * -1f * stm.ActivePath.LaneWidth;
                Debug.DrawRay(leftRaycastOrigin, raycastDirection * stm.FrontDetectionRange, Color.magenta, 0.5f);
                bool canSwitchToLeft = !Physics.Raycast(leftRaycastOrigin, raycastDirection, stm.FrontDetectionRange + 2f, vehicle);
                if (canSwitchToLeft) {
                    Debug.Log("Switching To Left");
                    stm.TurnDirection = VehicleSTM.LEFT;
                    stm.ChangeState("VehicleSwitchLane");
                    return;
                }
            }

            // check right lane
            bool rightLaneAvailable = stm.LaneNumber > 0;
            if (rightLaneAvailable) {
                Vector3 rightRaycastOrigin = raycastOrigin + stm.transform.right * 1f * stm.ActivePath.LaneWidth;
                Debug.DrawRay(rightRaycastOrigin, raycastDirection * stm.FrontDetectionRange, Color.magenta, 0.5f);
                bool canSwitchToRight = !Physics.Raycast(rightRaycastOrigin, raycastDirection, stm.FrontDetectionRange + 2f, vehicle);
                if (canSwitchToRight) {
                    Debug.Log("Switching To Right");
                    stm.TurnDirection = VehicleSTM.RIGHT;
                    stm.ChangeState("VehicleSwitchLane");
                    return;
                }
            }

            // can't switch lane, stop
            stm.StopAllCoroutines();
            stm.ChangeState("VehicleStopping");
            return;
        }
        #endregion        
        
        #region transition if there is an intersection
        
        bool intersectionAhead = Physics.Raycast(raycastOrigin, raycastDirection, stm.FrontDetectionRange, intersection);
        if (intersectionAhead) {
            // wait for green light
            stm.StopAllCoroutines();
            stm.ChangeState("VehicleStopping");
            return;
        }

        #endregion
    }
}
