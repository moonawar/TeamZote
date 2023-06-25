using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "VehicleStopping", menuName = "TrafficSystem/States/VehicleStopping")]
public class VehicleStopping : BaseState<VehicleSTM>
{
    private float frontObstacleDist;
    public override void OnEnter(VehicleSTM stm)
    {
        Vector3 raycastOrigin = stm.VehicleBound.bounds.center + stm.VehicleBound.bounds.extents.x * stm.transform.forward;
        Vector3 raycastDirection = stm.transform.forward; 

        RaycastHit hitInfo;
        LayerMask layerMask = LayerMask.GetMask(new string[] {"Vehicle", "TrafficIntersection"});
        Physics.Raycast(raycastOrigin, raycastDirection, out hitInfo, stm.FrontDetectionRange, layerMask);
        frontObstacleDist = hitInfo.distance;

        stm.StartCoroutine(SlowingDown(stm));
    }

    public override void OnUpdate(VehicleSTM stm)
    {
        CheckTransition();
    }

    public override void OnExit(VehicleSTM stm) 
    {

    }

    public IEnumerator SlowingDown(VehicleSTM stm) {
        float currentObsDist = frontObstacleDist;
        while (currentObsDist > stm.StopAtDistance) {
            stm.Lerp_t += Time.deltaTime * stm.Speed / stm.ActivePath.Distance;

            float speed_t = Mathf.InverseLerp(frontObstacleDist, stm.StopAtDistance, currentObsDist);
            float currentSpeed = Mathf.Lerp(stm.Speed, 0, speed_t);
            stm.transform.position += stm.ActivePath.Direction * Time.deltaTime * currentSpeed;
            currentObsDist -= Time.deltaTime * currentSpeed;
            yield return null;
        }
    }

    private void CheckTransition() {
        // if there is no vehicle in front
        // transition to VehicleMoving
    }
}
