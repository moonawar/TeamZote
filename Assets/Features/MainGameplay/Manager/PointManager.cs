using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PointManager : MonoBehaviour
{
    public int StartingPoint = 50;
    public int Points {get; private set;}
    
    public void AddPoints(int points) {
        Points += points;
        if (Points < 0) {
            Points = 0;
        }
    }

    public void ResetPoints() {
        Points = StartingPoint;
    }

    public void SetPoints(int points) {
        Points = points;
    }
}
