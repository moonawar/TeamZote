using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ViolationType 
{
    NoHelmet,
    Underage,
    TLightViolation,
    Crash,
    CounterFlow,
    SignViolation,
    NoPlate,
    NoLicense,
    NoRearviewMirror,
    ExcessivePassenger,
}

[CreateAssetMenu(fileName = "TrfViolation", menuName = "TrafficSystem/TrfViolation")]
public class TFCViolation : ScriptableObject {
    public ViolationType ViolationType;
    public int PointsOnTicket;
    public int PointsOnScold;
    public int PointsOnWrong;
    public bool IsHappening;
}

