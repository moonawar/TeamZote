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

    [HideInInspector] public float OffsetScale;
    [HideInInspector] public Vector3 Direction;
    [HideInInspector] public Vector3 Perpendicular;

    private void Awake() {
        if (StartPosition == null || TargetPosition == null) {
            return;
        }

        OffsetScale = (NumberOfLanes - 1) / 2f;
        Direction = (TargetPosition.position - StartPosition.position).normalized;
        Perpendicular = Vector3.Cross(Direction, Vector3.up);
    }

#if UNITY_EDITOR
    public void DrawConnection() {
        if (StartPosition == null || TargetPosition == null) {
            return;
        }

        float offsetScale = (NumberOfLanes - 1) / 2f;
        Vector3 direction = (TargetPosition.position - StartPosition.position).normalized;
        Vector3 perpendicular = Vector3.Cross(direction, Vector3.up);

        for (float s = -1f * offsetScale; s <= offsetScale; s++) {
            Vector3 offset = perpendicular * s * LaneWidth;
            Gizmos.color = LineColor;
            Gizmos.DrawLine(StartPosition.position + offset, TargetPosition.position + offset);
        }
    }
#endif
}