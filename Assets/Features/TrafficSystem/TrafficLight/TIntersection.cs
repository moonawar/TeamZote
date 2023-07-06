using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class LaneTrafficState {
    public LaneTLData laneTrafficData;
    public TLColor currentLight;
    public MapConnection thisLane;
    public float timeRemaining;
    public bool allowedToTurnLeft = true;
    public Action OnTrafficLightGreen;
}

public enum TLColor {
    Red,
    Yellow,
    Green
}

public class TIntersection : MonoBehaviour
{
    [SerializeField] private List<LaneTrafficState> lanes;
    private int currentLaneIndex = 0; // the index of the lane that is currently green
    private LaneTrafficState activeLane; // the lane that is currently green

    private void Start() {
        foreach (LaneTrafficState lane in lanes) {
            lane.timeRemaining = 0;
            lane.currentLight = TLColor.Red;
        }

        activeLane = lanes[currentLaneIndex];
        activeLane.currentLight = TLColor.Green;
        activeLane.OnTrafficLightGreen?.Invoke();
        activeLane.timeRemaining = activeLane.laneTrafficData.GreenLight.duration;
    }

    // Update is called once per frame
    void Update()
    {
        if (activeLane.currentLight == TLColor.Green) {
            activeLane.timeRemaining -= Time.deltaTime;
            if (activeLane.timeRemaining <= 0) {
                StartCoroutine(ChangeLane());
            }     
        }
    }

    public LaneTrafficState GetLaneTrafficState(MapConnection lane) {
        return lanes.Find(l => l.thisLane == lane);
    }

    private IEnumerator ChangeLane() {
        activeLane.currentLight = TLColor.Yellow;
        activeLane.timeRemaining = activeLane.laneTrafficData.YellowLight.duration;

        while (activeLane.timeRemaining > 0) {
            activeLane.timeRemaining -= Time.deltaTime;
            yield return null;
        }
        activeLane.currentLight = TLColor.Red;
        
        yield return new WaitForSeconds(4f);
        currentLaneIndex = (currentLaneIndex + 1) % lanes.Count;
        activeLane = lanes[currentLaneIndex];
        activeLane.currentLight = TLColor.Green;
        activeLane.OnTrafficLightGreen?.Invoke();
        activeLane.timeRemaining = activeLane.laneTrafficData.GreenLight.duration;
    }
}
