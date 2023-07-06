using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightClue : MonoBehaviour
{
    [SerializeField] private Material redLight;
    [SerializeField] private Material yellowLight;
    [SerializeField] private Material greenLight;

    [SerializeField] private TIntersection intersection;
    [SerializeField] private MapConnection path;
    private LaneTrafficState laneState;

    private TLColor lcBuffer;
    private TLColor lightColor;

    private Renderer rend;

    private bool safeUpdate = false;

    private void Start()
    {
        if (intersection == null)
        {
            Debug.LogWarning("No intersection assigned to traffic light clue");
            return;
        } 

        if (path == null)
        {
            Debug.LogWarning("No path assigned to traffic light clue");
            return;
        }

        laneState = intersection.GetLaneTrafficState(path);
        lcBuffer = TLColor.Red;
        lightColor = TLColor.Red;

        if (redLight == null || yellowLight == null || greenLight == null)
        {
            Debug.LogWarning("Not all light materials assigned to traffic light clue");
            return;
        }

        rend = GetComponent<Renderer>();
        rend.material = redLight;

        safeUpdate = true;
    }

    private void Update() {
        if (!safeUpdate) return;

        lightColor = laneState.currentLight;
        if (lightColor != lcBuffer) {
            switch (lightColor) {
                case TLColor.Red:
                    rend.material = redLight;
                    break;
                case TLColor.Yellow:
                    rend.material = yellowLight;
                    break;
                case TLColor.Green:
                    rend.material = greenLight;
                    break;
            }
            lcBuffer = lightColor;
        }
    }
}
