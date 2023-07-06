using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrafficActor : MonoBehaviour
{
    [Header("Driver Info")]
    [SerializeField] private Sprite actorSprite;
    public string DriversName;
    public int Age;
    public string LicenseNumber;

    [Header("Vehicle Info")]
    public string VehiclePlateNumber;
    public VehicleCap VehicleCapacity;
    [HideInInspector] public List<TFCViolation> Violations;
}

[CreateAssetMenu(fileName = "VehicleCap", menuName = "TrafficSystem/VehicleCap")]
public class VehicleCap : ScriptableObject
{
    public int NumOfPassengers; 
    public int MaxPassengers;
}
