using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VehicleSTM : BaseStateMachine<VehicleSTM>
{
    #region stm_data

    [Header("Vehicle Settings")]
    public float TopSpeed = 8f;
    public float Acceleration = 6.4f;
    // public float AccelerationTime = 1.2f;
    [Tooltip("Distance from the front obstacle where the vehicle stops")]
    public float StopAtDistance = 1f;
    public float TurnAngle = 30f;
    public float FrontDetectionRange = 8f;
    public float CurrentSpeed = 0f;

    public const int LEFT = -1;
    public const int RIGHT = 1;
    [HideInInspector] public int TurnDirection = 0;
    [HideInInspector] public bool IgnoreTL = false;
    [HideInInspector] public int VEHICLE_LM;

    [HideInInspector] public int TINTERSECTION_LM;
    [HideInInspector] public int OBSTACLE_LM;

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

        VEHICLE_LM = LayerMask.GetMask("Vehicle");
        TINTERSECTION_LM = LayerMask.GetMask("TrafficIntersection");
        OBSTACLE_LM = LayerMask.GetMask(new string[] {"Vehicle", "TrafficIntersection"});

        StopAtDistance += Random.Range(-0.3f, 0.3f);
        FrontDetectionRange += Random.Range(-0.5f, 0.5f);

        IgnoreTL = false;

        TriggerEventHandler colliderEvent = VehicleBound.GetComponent<TriggerEventHandler>();
        colliderEvent.OnTriggerEnterEvent += OnTriggerEnter; 
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

        Vector3 startingMapPoint = path.GetLaneStartingPoint(laneNumber);
        Vector3 targetMapPoint = path.GetLaneTargetPoint(laneNumber);
        
        transform.position = startingMapPoint;
        transform.LookAt(targetMapPoint);

        currentState.OnEnter(Data);
    }

    public Vector3 GetRaycastOrig() {
        return VehicleBound.bounds.center + VehicleBound.bounds.extents.x * transform.forward;
    }

    public float GetDetectionRange() {
        return (CurrentSpeed / TopSpeed) * FrontDetectionRange;
    }

#if UNITY_EDITOR
    private void OnDrawGizmos() {
        Vector3 raycastOrigin = VehicleBound.bounds.center + VehicleBound.bounds.extents.x * transform.forward;
        Vector3 raycastDirection = transform.forward; 
        float detectionRange = (CurrentSpeed / TopSpeed) * FrontDetectionRange;
        Debug.DrawRay(raycastOrigin, raycastDirection * detectionRange, Color.red);
    }
#endif
}
