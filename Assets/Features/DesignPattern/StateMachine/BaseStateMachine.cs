using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct BaseStateIdentifier<T>
{
    public string Name;
    public BaseState<T> State;
}

public abstract class BaseStateMachine<T> : MonoBehaviour
{
    [SerializeField] protected BaseState<T> initialState;
    public BaseState<T> currentState;
    [HideInInspector] public T Data;
    public List<BaseStateIdentifier<T>> States;

    virtual protected void Awake() {
        if (initialState == null) Debug.LogWarning("Initial state is not set for " + gameObject.name + "( " + GetHashCode() + " )");
        currentState = initialState;
    }

    virtual protected void Start() {
        currentState?.OnEnter(Data);
    }

    virtual protected void Update() {
        currentState?.OnUpdate(Data);
    }

    virtual protected void FixedUpdate() {
        currentState?.OnFixedUpdate(Data);
    }

    virtual protected void LateUpdate() {
        currentState?.OnSLateUpdate(Data);
    }

    virtual protected void OnTriggerEnter(Collider other) {
        currentState?.OnSTriggerEnter(Data, other);
    }

    virtual protected void OnTriggerExit(Collider other) {
        currentState?.OnSTriggerExit(Data, other);
    }

    virtual protected void OnCollisionEnter(Collision other) {
        currentState?.OnSCollisionEnter(Data, other);
    }
    virtual protected void OnCollisionExit(Collision other) {
        currentState?.OnSCollisionExit(Data, other);
    }


    virtual public void ChangeState(string Name) {
        BaseState<T> state = States.Find(s => s.Name == Name).State;
        if (state == null) {
            Debug.LogWarning("State " + Name + " not found in " + gameObject.name);
            return;
        }

        currentState?.OnExit(Data);
        currentState = state;
        currentState?.OnEnter(Data);
    }
}
