using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR
public class Map : MonoBehaviour
{
    [SerializeField] private Transform connectionsParent;
    [SerializeField] private bool drawGizmos = true;

    // private void OnDrawGizmos() {
    //     if (!drawGizmos) return;
    //     foreach (MapConnection connection in connectionsParent.GetComponentsInChildren<MapConnection>()) {
    //         if (connection != null) connection.DrawConnection();
    //     }
    // }
}
#endif