using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class MapConnection : MonoBehaviour
{
    public Transform StartPosition;
    public Transform TargetPosition;
    public int NumberOfLanes = 2;
    public float LaneWidth = 1.5f;
    public Color32 LineColor = Color.white;
    public Color32 SelectedColor = Color.green;

    [HideInInspector] public float OffsetScale;
    [HideInInspector] public Vector3 Direction;
    [HideInInspector] public Vector3 Perpendicular;
    [HideInInspector] public float Distance;

    private void Awake() {
        if (StartPosition == null || TargetPosition == null) {
            return;
        }

        OffsetScale = (NumberOfLanes - 1) / 2f;
        Direction = (TargetPosition.position - StartPosition.position).normalized;
        Distance = Vector3.Distance(StartPosition.position, TargetPosition.position);
        Perpendicular = Vector3.Cross(Direction, Vector3.up);
    }

    public Vector3 GetLaneStartingPoint(int laneNumber) {
        float offsetScale = -1f * OffsetScale;
        for (int i = 0; i < laneNumber; i++) {
            offsetScale += 1;
        }

        Vector3 offset = Perpendicular * offsetScale * LaneWidth;
        return StartPosition.position + offset;
    }

    public Vector3 GetLaneTargetPoint(int laneNumber) {
        float offsetScale = -1f * OffsetScale;
        for (int i = 0; i < laneNumber; i++) {
            offsetScale += 1;
        }

        Vector3 offset = Perpendicular * offsetScale * LaneWidth;
        return TargetPosition.position + offset;
    }

#if UNITY_EDITOR
    public void DrawConnection(bool selected = false) {
        if (StartPosition == null || TargetPosition == null) {
            return;
        }

        float offsetScale = (NumberOfLanes - 1) / 2f;
        Vector3 direction = (TargetPosition.position - StartPosition.position).normalized;
        Vector3 perpendicular = Vector3.Cross(direction, Vector3.up);

        for (float s = -1f * offsetScale; s <= offsetScale; s++) {
            Vector3 offset = perpendicular * s * LaneWidth;
            Gizmos.color = selected ? SelectedColor : LineColor;
            Gizmos.DrawLine(StartPosition.position + offset, TargetPosition.position + offset);
        }
    }

    private void OnDrawGizmos() {
        if (Object.FindObjectOfType<Map>().drawGizmos) DrawConnection();
    }

    private void OnDrawGizmosSelected() {
        DrawConnection(true);
    }
#endif
}