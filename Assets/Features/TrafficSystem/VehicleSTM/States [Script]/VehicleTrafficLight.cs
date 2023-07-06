using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "VehicleTrafficLight", menuName = "TrafficSystem/States/VehicleTrafficLight")]
public class VehicleTrafficLight : BaseState<VehicleSTM>
{
    private TIntersection trafficIntersection;
    private LaneTrafficState laneTrafficState;
    private VehicleSTM stm;
    public override void OnEnter(VehicleSTM stm)
    {
        Vector3 raycastOrigin = stm.VehicleBound.bounds.center + stm.VehicleBound.bounds.extents.x * stm.transform.forward;
        Vector3 raycastDirection = stm.transform.forward; 

        RaycastHit hitInfo;
        LayerMask layerMask = LayerMask.GetMask(new string[] {"TrafficIntersection"});
        Physics.Raycast(raycastOrigin, raycastDirection, out hitInfo, stm.FrontDetectionRange, layerMask);   
        trafficIntersection = hitInfo.collider.GetComponent<TIntersection>();

        this.stm = stm;

        laneTrafficState = trafficIntersection.GetLaneTrafficState(stm.ActivePath);
        laneTrafficState.OnTrafficLightGreen += OnTrafficLightGreen;
    }

    private void OnTrafficLightGreen() {
        Debug.Log("Light is green");
        laneTrafficState.OnTrafficLightGreen -= OnTrafficLightGreen;
        stm.ChangeState("VehicleMoving");
        return;
    }


    public override void OnExit(VehicleSTM stm)
    {
    }

    public override void OnUpdate(VehicleSTM stm)
    {
    }
}
