/* =============== ARCHIVED =============== */

// using System.Collections;
// using System.Collections.Generic;
// using UnityEngine;

// [CreateAssetMenu(fileName = "VehicleSwitchLane", menuName = "TrafficSystem/States/VehicleSwitchLane")]
// public class VehicleSwitchLane : BaseState<VehicleSTM>
// {
//     public override void OnEnter(VehicleSTM stm) {
//         stm.StartCoroutine(SwitchLane(stm));
//     }

//     // private IEnumerator Decelerate(VehicleSTM stm) {
//     //     float startingSpeed = stm.CurrentSpeed;
//     //     float duration = 0.2f;
//     //     float deceleration = 2f;

//     //     float elapsedTime = 0;
//     //     while (elapsedTime < duration) {
//     //         elapsedTime += Time.fixedDeltaTime;
//     //         stm.CurrentSpeed -= deceleration * Time.fixedDeltaTime;
//     //         stm.transform.position += stm.transform.forward * Time.fixedDeltaTime * stm.CurrentSpeed;
//     //         yield return new WaitForFixedUpdate();
//     //     }
//     //     stm.StartCoroutine(SwitchLane(stm));
//     // }

//     private float Clamp0(float value) {
//         if (value < 0) {
//             return 0;
//         } else {
//             return value;
//         }
//     }

//     private IEnumerator SwitchLane(VehicleSTM stm) {
//         #region  variables
//         float turnAngle = stm.TurnAngle;
//         float laneWidth = stm.ActivePath.LaneWidth;
//         float initSpeed = stm.CurrentSpeed;
//         #endregion

//         float t = 0f;
//         const float TURN_CONSTANT = 56.1f;

//         Vector3 startingPosition = stm.transform.position;
//         // Vector3 targetPosition = stm.transform.position + stm.transform.right * laneWidth + stm.transform.forward * (laneWidth);

//         float startingTargetDist = 2 * (TURN_CONSTANT / 360) *  Mathf.PI * 2 * laneWidth / Mathf.Sin(turnAngle * Mathf.Deg2Rad);
//         float switchDirection = stm.TurnDirection;

//         Vector3 startingRotation = stm.transform.rotation.eulerAngles;

//         while (t < 1f) {
//             float rotation;
//             if (t < 0.5f) {
//                 rotation = Mathf.Lerp(startingRotation.y, startingRotation.y + (switchDirection * turnAngle), t * 2);
//                 stm.transform.rotation = Quaternion.Euler(0, rotation, 0);
//             } else {
//                 rotation = Mathf.Lerp(startingRotation.y + (switchDirection * turnAngle), startingRotation.y, (t - 0.5f) * 2);
//                 stm.transform.rotation = Quaternion.Euler(0, rotation, 0);
//             }

//             if (t > 0.8f) {
//                 stm.CurrentSpeed = Mathf.Lerp(initSpeed, initSpeed/10f, (t - 0.8f) * 5);
//             }

//             stm.transform.position += stm.transform.forward * Time.fixedDeltaTime * stm.CurrentSpeed;

//             t += (Time.fixedDeltaTime * stm.CurrentSpeed) / startingTargetDist;
//             yield return new WaitForFixedUpdate();
//         }
        
//         stm.transform.rotation = Quaternion.Euler(0, startingRotation.y, 0);
//         stm.StopAllCoroutines();
//         stm.ChangeState("VehicleMoving");
//     }

//     public override void OnUpdate(VehicleSTM stm) {
//         // nothing
//     }
//     public override void OnExit(VehicleSTM stm) {
//         // nothing
//     }
    
// }
