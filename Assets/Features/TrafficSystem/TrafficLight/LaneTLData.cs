using UnityEngine;

[CreateAssetMenu(fileName = "TLData", menuName = "TrafficSystem/TLData")]
public class LaneTLData : ScriptableObject {
    public TLData YellowLight = new TLData(){trafficLightState = TLColor.Yellow};
    public TLData GreenLight = new TLData(){trafficLightState = TLColor.Green};
}

[System.Serializable]
public class TLData { // traffic light data
    public TLColor trafficLightState;
    public float duration;
    public Material lightMaterial;
}

