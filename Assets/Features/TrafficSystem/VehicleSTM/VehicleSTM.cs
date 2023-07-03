using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VehicleSTM : BaseStateMachine<VehicleSTM>
{
    #region stm_data

    [Header("Vehicle Settings")]
    public float TopSpeed = 8f;
    public float AccelerationTime = 1.2f;
    [Tooltip("Distance from the front obstacle where the vehicle stops")]
    public float StopAtDistance = 1f;
    public float TurnAngle = 30f;
    public float FrontDetectionRange = 8f;
    [HideInInspector] public float CurrentSpeed = 0f;

    public const int LEFT = -1;
    public const int RIGHT = 1;
    [HideInInspector] public int TurnDirection = 0;

    #endregion

    #region references

    [Header("References")]
    public BoxCollider VehicleBound; 
    [HideInInspector] public MapConnection ActivePath;
    [HideInInspector] public int LaneNumber;

    #endregion

    override protected void Awake() {
        base.Awake();
        Data = this;
    }

    override protected void Start() {
        /**
            Subtituted by StartVehicle() method, called by VehicleSpawner
            init variables only
        */
    }

    override protected void Update() {
        base.Update();
    }

    public void StartVehicle(MapConnection path, int laneNumber) {
        ActivePath = path;
        LaneNumber = laneNumber;

        Vector3 startingMapPoint = ActivePath.StartPosition.position;
        Vector3 targetMapPoint = ActivePath.TargetPosition.position;

        
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

        currentState.OnEnter(Data);
    }

#if UNITY_EDITOR
    private void OnDrawGizmos() {
        Vector3 raycastOrigin = VehicleBound.bounds.center + VehicleBound.bounds.extents.x * transform.forward;
        Vector3 raycastDirection = transform.forward; 
        Debug.DrawRay(raycastOrigin, raycastDirection * FrontDetectionRange, Color.red);
    }
#endif
}
