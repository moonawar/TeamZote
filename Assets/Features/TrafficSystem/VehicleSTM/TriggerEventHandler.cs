using UnityEngine;

public class TriggerEventHandler : MonoBehaviour
{
    public delegate void TriggerEnterEvent(Collider other);
    public event TriggerEnterEvent OnTriggerEnterEvent;

    private void OnTriggerEnter(Collider other)
    {
        OnTriggerEnterEvent?.Invoke(other);
    }

}