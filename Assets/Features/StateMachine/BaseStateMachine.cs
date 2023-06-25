using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseStateMachine<T> : MonoBehaviour
{
    [SerializeField] protected BaseState<T> initialState;
    protected BaseState<T> currentState;
    [HideInInspector] public T Data;

    virtual protected void Awake() {
        if (initialState == null) Debug.LogWarning("Initial state is not set for " + gameObject.name);
        currentState = initialState;
    }

    virtual protected void Start() {
        currentState?.OnEnter(Data);
    }

    virtual protected void Update() {
        currentState?.OnUpdate(Data);
    }

    virtual protected void ChangeState(BaseState<T> state) {
        currentState?.OnExit(Data);
        currentState = state;
        currentState?.OnEnter(Data);
    }
}
