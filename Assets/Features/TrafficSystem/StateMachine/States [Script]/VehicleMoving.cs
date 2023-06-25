using System.Collections;
using System.Collections.Generic;
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
        startingMapPoint = stm.ActivePath.StartPosition.position;
        targetMapPoint = stm.ActivePath.TargetPosition.position;

        int laneNumber = stm.LaneNumber;
        MapConnection path = stm.ActivePath;
        Transform transform = stm.transform;
        
        // convert lane number to offset scale
        float offsetScale = -1f * path.OffsetScale;
        for (int i = 0; i < laneNumber; i++ ) {
            offsetScale += 1;
        };

        // calculate offset
        Vector3 offset = path.Perpendicular * offsetScale * path.LaneWidth;

        startingMapPoint = path.StartPosition.position + offset;
        targetMapPoint = path.TargetPosition.position + offset;

        transform.position = startingMapPoint + path.Direction * stm.Lerp_t;
        transform.LookAt(targetMapPoint);
    }

    public override void OnUpdate(VehicleSTM stm)
    {
        stm.Lerp_t += Time.deltaTime * stm.Speed / stm.ActivePath.Distance;
        stm.transform.position += stm.ActivePath.Direction * Time.deltaTime * stm.Speed;

        CheckTransition(stm);
    }

    public override void OnExit(VehicleSTM stm)
    {
        //nothing
    }

    private void CheckTransition(VehicleSTM stm) {
        
        Vector3 raycastOrigin = stm.VehicleBound.bounds.center + stm.VehicleBound.bounds.extents.x * stm.transform.forward;
        Vector3 raycastDirection = stm.transform.forward; 

        #region transition if there is vehicle in front
        
        // check vehicle in front
        LayerMask layerMask = LayerMask.GetMask("Vehicle");

        bool vehicleAhead = Physics.Raycast(raycastOrigin, raycastDirection, stm.FrontDetectionRange, layerMask);
        if (vehicleAhead) {
            // check if lane switch is possible

            // check left lane
            // bool leftLaneAvailable = stm.LaneNumber > 0;
            // if (leftLaneAvailable) {
            //     Vector3 leftRaycastOrigin = raycastOrigin + stm.ActivePath.Perpendicular * -1f * stm.ActivePath.LaneWidth;
            //     bool canSwitchToLeft = !Physics.Raycast(leftRaycastOrigin, raycastDirection, stm.FrontDetectionRange, layerMask);
            //     if (canSwitchToLeft) {
            //         Debug.Log("Switch To Left"); // harusnya pindah state
            //         stm.LaneNumber -= 1; 
            //         stm.transform.position += stm.ActivePath.Perpendicular * -1f * stm.ActivePath.LaneWidth;
            //         return;
            //     }
            // }

            // // check right lane
            // bool rightLaneAvailable = stm.LaneNumber < stm.ActivePath.NumberOfLanes - 1;
            // if (rightLaneAvailable) {
            //     Vector3 rightRaycastOrigin = raycastOrigin + stm.ActivePath.Perpendicular * 1f * stm.ActivePath.LaneWidth;
            //     bool canSwitchToRight = !Physics.Raycast(rightRaycastOrigin, raycastDirection, stm.FrontDetectionRange, layerMask);
            //     if (canSwitchToRight) {
            //         Debug.Log("Switch To Right"); // harusnya pindah state
            //         stm.LaneNumber += 1;
            //         stm.transform.position += stm.ActivePath.Perpendicular * 1f * stm.ActivePath.LaneWidth;
            //         return;
            //     }
            // }

            // can't switch lane, stop
            stm.ChangeState("VehicleStopping");
            return;
        }
        #endregion        
        
        #region transition if there is an intersection
        #endregion
    }
}
