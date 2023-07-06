using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface TFCViolationEvent 
{
    void Trigger(VehicleSTM stm);
}