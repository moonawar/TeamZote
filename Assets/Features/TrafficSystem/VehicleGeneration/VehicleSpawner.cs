using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VehicleSpawner : MonoBehaviour
{
    // Simple vehicle spawner, not finished
    [SerializeField] private GameObject vehiclePrefab;
    [SerializeField] private MapConnection path;
    [SerializeField] private Transform vehiclesParent;

    private void SpawnVehicle() {
        if (vehiclePrefab == null || path == null || vehiclesParent == null) {
            Debug.LogWarning("Spawner not set up correctly");
            return;
        }

        GameObject vehicle = Instantiate(vehiclePrefab, path.StartPosition.position, Quaternion.identity, vehiclesParent);
        VehicleSTM vehicleSTM = vehicle.GetComponent<VehicleSTM>();

        if (vehicleSTM == null) {
            Debug.LogWarning("Vehicle prefab does not have a VehicleSTM component");
            return;
        }

        Random.InitState(System.DateTime.Now.Millisecond);
        int laneNumber = Random.Range(0, path.NumberOfLanes);
        
        vehicleSTM.StartVehicle(path, 0);
    }

    private void Update() {
        if (Input.GetKeyDown(KeyCode.Space)) {
            SpawnVehicle();
        }
    }
}