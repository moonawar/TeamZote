using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VehicleSTM : BaseStateMachine<VehicleSTM>
{
    #region stm_data
    [Header("Vehicle Settings")]
    public float Speed = 1f;

    [Tooltip("Distance from the front of the vehicle to the point where the vehicle starts to slow down")]
    public float FrontDetectionRange = 5f;

    [Tooltip("Distance from the front obstacle where the vehicle stops")]
    public float StopAtDistance = 1f;


    [Header("References")]
    public BoxCollider VehicleBound; 


    [HideInInspector] public MapConnection ActivePath;
    [HideInInspector] public int LaneNumber;
    [HideInInspector] [Range(0f, 1f)] public float Lerp_t;
    #endregion

    override protected void Awake() {
        base.Awake();
        Data = this;
    }

    override protected void Start() {
        /**
            Subtituted by StartVehicle() method, called by VehicleSpawner
        */
    }

    public void StartVehicle(MapConnection path, int laneNumber) {
        ActivePath = path;
        LaneNumber = laneNumber;
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
