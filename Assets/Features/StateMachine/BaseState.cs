using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseState<T> : ScriptableObject
{
    public abstract void OnEnter(T data);
    public abstract void OnUpdate(T data);
    public abstract void OnExit(T data);
}
