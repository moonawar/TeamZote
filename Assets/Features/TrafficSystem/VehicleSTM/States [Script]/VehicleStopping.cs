using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "VehicleStopping", menuName = "TrafficSystem/States/VehicleStopping")]
public class VehicleStopping : BaseState<VehicleSTM>
{
    private float frontObstacleDist;
    private Vector3 raycastOrigin; // for obstacle detection

    public override void OnEnter(VehicleSTM stm)
    {
        CheckTransition(stm);
        
        Vector3 raycastOrigin = stm.VehicleBound.bounds.center + stm.VehicleBound.bounds.extents.x * stm.transform.forward;
        Vector3 raycastDirection = stm.transform.forward; 

        RaycastHit hitInfo;
        LayerMask layerMask = LayerMask.GetMask(new string[] {"Vehicle", "TrafficIntersection"});
        Physics.Raycast(raycastOrigin, raycastDirection, out hitInfo, stm.FrontDetectionRange, layerMask);
        frontObstacleDist = hitInfo.distance;

        stm.StartCoroutine(Decelerate(stm));
    }

    public override void OnUpdate(VehicleSTM stm)
    {
        CheckTransition(stm);
    }

    public override void OnExit(VehicleSTM stm) 
    {
        // do nothing
    }

    public IEnumerator Decelerate(VehicleSTM stm) {
        float currentObsDist = frontObstacleDist;
        float previousTopSpeed = stm.CurrentSpeed;
        while (currentObsDist > stm.StopAtDistance) {
            Vector3 raycastOrigin = stm.VehicleBound.bounds.center + stm.VehicleBound.bounds.extents.x * stm.transform.forward;
            Vector3 raycastDirection = stm.transform.forward;

            float speed_t = Mathf.InverseLerp(frontObstacleDist, stm.StopAtDistance, currentObsDist);
            stm.CurrentSpeed = Mathf.Lerp(previousTopSpeed, 0, speed_t);
            stm.transform.position += stm.ActivePath.Direction * Time.fixedDeltaTime * stm.CurrentSpeed;
            currentObsDist -= Time.fixedDeltaTime * stm.CurrentSpeed;
            yield return new WaitForFixedUpdate();
        }
    }

    private void CheckTransition(VehicleSTM stm) {
        Vector3 raycastOrigin = stm.VehicleBound.bounds.center + stm.VehicleBound.bounds.extents.x * stm.transform.forward;
        LayerMask layerMask = LayerMask.GetMask(new string[] {"Vehicle", "TrafficIntersection"});
        bool obstacleAhead = Physics.Raycast(raycastOrigin, stm.transform.forward, stm.FrontDetectionRange, layerMask);
        if (!obstacleAhead) {
            stm.StopAllCoroutines();
            Debug.Log("Vehicle is back to moving");
            stm.ChangeState("VehicleMoving");
        }
    }
}
