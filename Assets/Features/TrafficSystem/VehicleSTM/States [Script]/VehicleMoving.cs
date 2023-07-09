using System;
using System.Collections;
using UnityEngine;
using Random = UnityEngine.Random;

[CreateAssetMenu(fileName = "VehicleMoving", menuName = "TrafficSystem/States/VehicleMoving")]
public class VehicleMoving : BaseState<VehicleSTM>
{
    #region variables
    private Vector3 startingMapPoint;
    private Vector3 targetMapPoint;
    private float deceleration;
    private Action<VehicleSTM> OnVehicleStopped;
    private bool isStopped = false;

    #endregion

    public override void OnEnter(VehicleSTM stm)
    {
        stm.StartCoroutine(CalculateSpeed(stm));
    }

    public override void OnFixedUpdate(VehicleSTM stm)
    {
        stm.transform.position += stm.transform.forward * Time.fixedDeltaTime * stm.CurrentSpeed;
    }

    private IEnumerator CalculateSpeed(VehicleSTM stm) {
        while (true) {
            RaycastHit hitInfo;
            float detectionRange = Mathf.Max(stm.StopAtDistance, stm.GetDetectionRange());

            if (!stm.IgnoreTL) {
                bool trfAhead = Physics.Raycast(stm.GetRaycastOrig(), stm.transform.forward, 
                    out hitInfo, detectionRange, stm.TINTERSECTION_LM);

                if (trfAhead) {
                    TIntersection trf = hitInfo.collider.GetComponent<TIntersection>();
                    LaneTrafficState laneTrfState = trf.GetLaneTrafficState(stm.ActivePath);

                    if (laneTrfState != null) {                    
                        if (laneTrfState.currentLight == TLColor.Green || laneTrfState.currentLight == TLColor.Yellow) {
                            stm.IgnoreTL = true;
                        }
                    } else {
                        stm.IgnoreTL = true;
                    }
                }
            }

            LayerMask obstacle = stm.IgnoreTL ? stm.VEHICLE_LM : stm.OBSTACLE_LM;
            bool obstacleAhead = Physics.Raycast(stm.GetRaycastOrig(), stm.transform.forward, 
                out hitInfo, detectionRange, obstacle);

            
            float speed = stm.CurrentSpeed;
            if (!obstacleAhead) {
                if (isStopped) {
                    // randomize reaction time
                    float reactionTime = Random.Range(0f, 0.23f);
                    yield return new WaitForSeconds(reactionTime); // realistic delay
                    isStopped = false;
                }
                
                if (speed >= stm.TopSpeed) {
                    speed = stm.TopSpeed;
                }
                speed += Time.fixedDeltaTime * (stm.Acceleration + Random.Range(-0.8f, 0.8f));
                speed = Mathf.Clamp(speed, 0, stm.TopSpeed);
            } else {
                deceleration = (speed * speed) / (2 * Mathf.Clamp(hitInfo.distance - stm.StopAtDistance, 0.1f, 100));
                speed -= Time.fixedDeltaTime * (deceleration + Random.Range(0, 0.5f));
                if (speed <= 0.05f)  {
                    speed = 0;
                    if (!isStopped) {
                        isStopped = true;
                    }
                }
            }

            stm.CurrentSpeed = speed;
            yield return new WaitForFixedUpdate();
        }
    }

    private Vector3 CalculatePivotPoint(Vector3 from, Vector3 to) {
        Vector3 pivot = new Vector3(
            from.x,
            from.y,
            to.z
        );
        return pivot;
    }

    private IEnumerator SetTurnAngle(VehicleSTM stm, TIntersection trf, TurnDirection turnDirection) {
        while (Vector3.Distance(stm.transform.position, stm.ActivePath.GetLaneTargetPoint(stm.LaneNumber)) > 0.8f) {
            yield return new WaitForFixedUpdate();
        }

        Vector3 start = stm.ActivePath.GetLaneTargetPoint(stm.LaneNumber);
        
        MapConnection target = trf.GetNextLane(stm.ActivePath, turnDirection);
        Vector3 end = target.GetLaneStartingPoint(stm.LaneNumber);

        Debug.DrawLine(start, start + Vector3.up * 5, Color.cyan, 10);
        Debug.DrawLine(end, end + Vector3.up * 5, Color.cyan, 10);


        float angle = stm.transform.rotation.eulerAngles.y;
        float targetAngle;
        if (turnDirection == TurnDirection.Left) {
            targetAngle = -90;
        } else {
            targetAngle = 90;
        }

        targetAngle += angle;

        Vector3 pivot = CalculatePivotPoint(start, end);

        Debug.DrawLine(pivot, pivot + Vector3.up * 5, Color.red, 10);

        float radius = Vector3.Distance(start, end);

        float elapsedAngle = 0;
        float rotationSpeed = 0;

        while (elapsedAngle < 90) {
            rotationSpeed = stm.CurrentSpeed / radius;
            rotationSpeed *= Mathf.Rad2Deg;

            elapsedAngle += rotationSpeed * Time.fixedDeltaTime;
            float t = elapsedAngle / 90;
            float angleToRotate = Mathf.Lerp(angle, targetAngle, t);
            stm.transform.rotation = Quaternion.Euler(0, angleToRotate, 0);
            yield return new WaitForFixedUpdate();
        }
        stm.ActivePath = target;
    }

    public override void OnSTriggerEnter(VehicleSTM stm, Collider other)
    {
        if (other.CompareTag("TrafficIntersection")) {
            stm.IgnoreTL = false;
            
            TIntersection trf = other.GetComponent<TIntersection>();
            int laneNumber = stm.LaneNumber;
            int numberOfLanes = stm.ActivePath.NumberOfLanes;

            bool goesStraight = Random.Range(0, 4) < 3;

            if (laneNumber == 0 && !goesStraight) {
                stm.StartCoroutine(SetTurnAngle(stm, trf, TurnDirection.Right));
            } else if (laneNumber == numberOfLanes - 1 && !goesStraight) {
                stm.StartCoroutine(SetTurnAngle(stm, trf, TurnDirection.Left));
            } else {
                stm.ActivePath = trf.GetNextLane(stm.ActivePath, TurnDirection.Straight);
            }
        }
    }

    
}

#region  ================ ARCHIVE ================ 
    // public IEnumerator Accelerate(VehicleSTM stm) {
    //     float startingSpeed = stm.CurrentSpeed;
    //     float accelerationTime = stm.AccelerationTime * (stm.TopSpeed - startingSpeed) / stm.TopSpeed;

    //     float elapsedTime = 0;
    //     while (stm.CurrentSpeed < stm.TopSpeed) {
    //         elapsedTime += Time.fixedDeltaTime;
    //         stm.CurrentSpeed += Time.fixedDeltaTime * Mathf.Lerp(startingSpeed, stm.TopSpeed, elapsedTime / accelerationTime);
    //         yield return new WaitForFixedUpdate();
    //     }
    // }

    // private void CheckTransition(VehicleSTM stm) {
        // #region transition if there is vehicle in front
    
        // RaycastHit hitInfo;

        // // --- stopping condition only ----
        // // float detectionRange = Mathf.Max(stm.StopAtDistance, stm.GetDetectionRange());
        // // bool obstacleAhead = Physics.Raycast(stm.GetRaycastOrig(), stm.transform.forward, 
        // //     out hitInfo, stm.GetDetectionRange(), stm.TINTERSECTION_LM);

        // // if (obstacleAhead) {
        // //     stm.CurrentSpeed = 0;
        // // }
        // // --------------------------------
    
        // bool vehicleAhead = Physics.Raycast(stm.GetRaycastOrig(), 
        //     stm.transform.forward, out hitInfo, stm.FrontDetectionRange, stm.VEHICLE_LM);

        // if (vehicleAhead) {
        //     // check if lane switch is possible
            
        //     VehicleSTM vehicleHit = hitInfo.transform.parent.GetComponent<VehicleSTM>();

        //     if (!vehicleHit) {
        //         Debug.LogWarning("Vehicle hit is not a VehicleSTM");
        //         return;
        //     }

        //     if (vehicleHit.CurrentSpeed > stm.CurrentSpeed) {
        //         stm.StopAllCoroutines();
        //         stm.ChangeState("VehicleStopping");
        //         return;
        //     }

        //     // check left lane
        //     bool leftAvail = stm.LaneNumber < stm.ActivePath.NumberOfLanes - 1;
        //     if (leftAvail) {
        //         Vector3 lRayOrig = stm.GetRaycastOrig() + stm.transform.right * -1f * stm.ActivePath.LaneWidth;
        //         Debug.DrawRay(lRayOrig, stm.transform.forward * stm.FrontDetectionRange, Color.cyan, 0.5f);
        //         bool canSwitchL = !Physics.Raycast(lRayOrig, stm.transform.forward, stm.FrontDetectionRange + 2f, stm.VEHICLE_LM);
        //         canSwitchL = canSwitchL && Random.Range(0,4) == 0; // little randomizer

        //         if (canSwitchL) {
        //             stm.TurnDirection = VehicleSTM.LEFT;
        //             stm.LaneNumber++;
        //             stm.ChangeState("VehicleSwitchLane");
        //             return;
        //         }
        //     }

        //     // check right lane
        //     bool rightAvail = stm.LaneNumber > 0;
        //     if (rightAvail) {
        //         Vector3 rRayOrig = stm.GetRaycastOrig() + stm.transform.right * 1f * stm.ActivePath.LaneWidth;
        //         Debug.DrawRay(rRayOrig, stm.transform.forward * stm.FrontDetectionRange, Color.cyan, 0.5f);
        //         bool canSwitchToRight = !Physics.Raycast(rRayOrig, stm.transform.forward, stm.FrontDetectionRange + 2f, stm.VEHICLE_LM);
        //         canSwitchToRight = canSwitchToRight && Random.Range(0,4) == 0; // little randomizer

        //         if (canSwitchToRight) {
        //             stm.TurnDirection = VehicleSTM.RIGHT;
        //             stm.LaneNumber--;
        //             stm.ChangeState("VehicleSwitchLane");
        //             return;
        //         }
        //     }

        //     // not switching lane, stop then
        //     stm.StopAllCoroutines();
        //     stm.ChangeState("VehicleStopping");
        //     return;
        // }
        // #endregion        
        
        // #region transition if there is an intersection
        
        // LayerMask intersection = LayerMask.GetMask(new string[]{"TrafficIntersection"});
        // bool intersectionAhead = Physics.Raycast(stm.GetRaycastOrig(), rayDir, out hitInfo, stm.FrontDetectionRange / 2f, intersection);

        // if (intersectionAhead) {
        //     TIntersection intr = hitInfo.transform.GetComponent<TIntersection>();
        //     LaneTrafficState state = intr.GetLaneTrafficState(stm.ActivePath);

        //     if (state.currentLight == TLColor.Red) {
        //         stm.StopAllCoroutines();
        //         stm.ChangeState("VehicleStopping");
        //         return;
        //     }
            
        //     // else, go through the intersection by turning or move forward
        //     stm.IgnoreTL = true;
        //     return;
        // }

        // #endregion
    // }

    // private void OnTriggerEnter(Collider other) 
    // {
    //     if (other.gameObject.layer == LayerMask.NameToLayer("TrafficIntersection")) {
    //         thisSTM.IgnoreTL = false;
    //     }
    // }
#endregion