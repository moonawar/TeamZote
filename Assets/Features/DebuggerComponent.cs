using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebuggerComponent : MonoBehaviour
{
    public float TimeScale = 1f;

    // // Start is called before the first frame update
    // void Start()
    // {
    //     Debug.Log(Mathf.InverseLerp(10, 1, 12));
    // }

    // Update is called once per frame
    void Update()
    {
        Time.timeScale = TimeScale;   
    }
}