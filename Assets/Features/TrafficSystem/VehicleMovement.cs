using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VehicleMovement : MonoBehaviour
{
    [SerializeField] private float speed = 10f;
    [SerializeField] private float detectionRange = 2f;
    private Vector3 startingMapPoint;
    private Vector3 targetMapPoint;
    private int laneNumber = 0; // starting from 0, which is the leftmost lane

    public void StartMovement(MapConnection path, int laneNumber) {
        this.laneNumber = laneNumber;

        // convert lane number to offset scale
        float offsetScale = -1f * path.OffsetScale;
        for (int i = 0; i < laneNumber; i++ ) {
            offsetScale += 1;
        };

        // calculate offset
        Vector3 offset = path.Perpendicular * offsetScale * path.LaneWidth;

        startingMapPoint = path.StartPosition.position + offset;
        targetMapPoint = path.TargetPosition.position + offset;

        transform.position = startingMapPoint;
        transform.LookAt(targetMapPoint);
    }

    private void Update() {
        transform.position += transform.forward * speed * Time.deltaTime;
        if (Vector3.Distance(transform.position, targetMapPoint) < detectionRange) {
            Destroy(gameObject);
        }
    }
}
