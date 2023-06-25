using System.Collections;
using System.Collections.Generic;
using UnityEngine;


#if UNITY_EDITOR
public class SwitchLaneDebug : MonoBehaviour
{
    [SerializeField] [Range(0f, 1f)] private float lerp_t;
    [SerializeField] private float speed;
    [SerializeField] private Vector3 startPosition;
    [SerializeField] private float turnAngle;
    [SerializeField] private float laneWidth;
    [SerializeField] [Range(0f, 1f)] private float initialTurnaround = 0.2f;
    [SerializeField] private float animationDuration = 1.5f;

    [SerializeField] [Range(0f, 1f)] private float timeOnStartSlowingDown = 0.8f;
    [SerializeField] [Range(0f, 1f)] private float timeToStartSwitchingLane = 0.2f;
    [SerializeField] [Range(0f, 1f)] private float timeToFinishSwitchingLane = 0.8f;

    private void Update() {
        if (Input.GetKeyDown(KeyCode.L)) {
            Debug.Log("L pressed");
            ResetAnim();
        } else if (Input.GetKeyDown(KeyCode.K)) {
            Debug.Log("K pressed");
            StartCoroutine(StartAnim());
        }
    }



    private IEnumerator StartAnim() {
        Debug.Log("StartAnim");
        
        while (true) {
            lerp_t += Time.deltaTime / animationDuration;

            if (lerp_t >= 1f) {
                transform.position = startPosition;
                lerp_t = 0f;
                transform.rotation = Quaternion.Euler(0f, 0f, 0f);
                yield return new WaitForSeconds(.3f);
            }

            if (lerp_t < initialTurnaround) {
                transform.rotation = Quaternion.Lerp(
                    Quaternion.Euler(0f, 0f, 0f), // from
                    Quaternion.Euler(0f, turnAngle, 0f), // to
                    lerp_t * 1 / initialTurnaround // t
                );
            } else {
                transform.rotation = Quaternion.Lerp(
                    Quaternion.Euler(0f, turnAngle, 0f), // from 
                    Quaternion.Euler(0f, 0f, 0f), // to
                    (lerp_t - initialTurnaround) * 1 / (1- initialTurnaround) // t
                );
            }
            
            if (lerp_t >= timeOnStartSlowingDown) {
                float currentSpeed = Mathf.Lerp(speed, 0, (lerp_t - timeOnStartSlowingDown) * 1 / (1 - timeOnStartSlowingDown));
                transform.position += transform.forward * currentSpeed * Time.deltaTime;
            } else {
                transform.position += transform.forward * speed * Time.deltaTime;
            }

            yield return new WaitForFixedUpdate();
        }
    }

    private void ResetAnim() {
        transform.position = startPosition;
        lerp_t = 0f;
        transform.rotation = Quaternion.Euler(0f, 0f, 0f);

        StopAllCoroutines();
    }
    
}
#endif

        // transform.position = startPosition;

        // if (lerp_t < initialTurnaround) {
        //     transform.rotation = Quaternion.Lerp(
        //         Quaternion.Euler(0f, 0f, 0f), // from
        //         Quaternion.Euler(0f, turnAngle, 0f), // to
        //         lerp_t * 1 / initialTurnaround // t
        //     );
        // } else {
        //     transform.rotation = Quaternion.Lerp(
        //         Quaternion.Euler(0f, turnAngle, 0f), // from 
        //         Quaternion.Euler(0f, 0f, 0f), // to
        //         (lerp_t - initialTurnaround) * 1 / (1- initialTurnaround) // t
        //     );
        // }