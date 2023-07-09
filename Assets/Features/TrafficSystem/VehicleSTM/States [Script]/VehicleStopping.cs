/* =============== ARCHIVED =============== */

// using System.Collections;
// using System.Collections.Generic;
// using UnityEngine;

// [CreateAssetMenu(fileName = "VehicleStopping", menuName = "TrafficSystem/States/VehicleStopping")]
// public class VehicleStopping : BaseState<VehicleSTM>
// {
//     private float frontObstacleDist;
//     private Vector3 raycastOrigin; // for obstacle detection

//     public override void OnEnter(VehicleSTM stm)
//     {
//         // CheckTransition(stm);
        
//         // Vector3 raycastOrigin = stm.VehicleBound.bounds.center + stm.VehicleBound.bounds.extents.x * stm.transform.forward;
//         // Vector3 raycastDirection = stm.transform.forward; 

//         // RaycastHit hitInfo;
//         // LayerMask layerMask = LayerMask.GetMask(new string[] {"Vehicle", "TrafficIntersection"});
        
//         // Physics.Raycast(stm.GetRaycastOrig(), stm.transform.forward, out hitInfo, stm.FrontDetectionRange, layerMask);
//         // frontObstacleDist = hitInfo.distance;

//         stm.StartCoroutine(Decelerate(stm));
//     }

//     public override void OnUpdate(VehicleSTM stm)
//     {
//         CheckTransition(stm);
//     }

//     public override void OnExit(VehicleSTM stm) 
//     {
//         // do nothing
//     }

//     public IEnumerator Decelerate(VehicleSTM stm) {
//         float currentObsDist = frontObstacleDist;
//         float previousTopSpeed = Mathf.Max(stm.CurrentSpeed, stm.TopSpeed/2f);

//         Vector3 raycastOrigin;
//         Vector3 raycastDirection;

//         while (currentObsDist > stm.StopAtDistance) {
//             raycastOrigin = stm.VehicleBound.bounds.center + stm.VehicleBound.bounds.extents.x * stm.transform.forward;
//             raycastDirection = stm.transform.forward;

//             float speed_t = Mathf.InverseLerp(frontObstacleDist, stm.StopAtDistance, currentObsDist);
//             stm.CurrentSpeed = Mathf.Lerp(previousTopSpeed, 0, speed_t);
//             stm.transform.position += stm.ActivePath.Direction * Time.fixedDeltaTime * stm.CurrentSpeed;
//             currentObsDist -= Time.fixedDeltaTime * stm.CurrentSpeed;
//             yield return new WaitForFixedUpdate();
//         }
//         yield return null;
//     }

//     private void CheckTransition(VehicleSTM stm) {
//         // LayerMask intersection = LayerMask.GetMask(new string[] {"TrafficIntersection"});
//         // bool intersectionAhead = Physics.Raycast(raycastOrigin, stm.transform.forward, stm.FrontDetectionRange, intersection);
//         // if (intersectionAhead) {
//         //     // stm.StopAllCoroutines();
//         //     // stm.ChangeState("VehicleTrafficLight");
//         // }

//         bool obstacleAhead = Physics.Raycast(stm.GetRaycastOrig(), 
//             stm.transform.forward, stm.FrontDetectionRange, stm.OBSTACLE_LM);

//         if (!obstacleAhead) {
//             stm.StopAllCoroutines();
//             stm.ChangeState("VehicleMoving");
//         }
//     }
// }
