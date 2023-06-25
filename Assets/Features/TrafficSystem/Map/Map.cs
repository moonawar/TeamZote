using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR
public class Map : MonoBehaviour
{
    [SerializeField] private List<MapConnection> connections;

    private void OnDrawGizmos() {
        foreach (MapConnection connection in connections) {
            if (connection != null) connection.DrawConnection();
        }
    }
}
#endif