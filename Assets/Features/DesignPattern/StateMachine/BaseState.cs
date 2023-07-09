using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseState<T> : ScriptableObject
{
    public virtual void OnEnter(T data) {}
    public virtual void OnUpdate(T data) {}
    public virtual void OnFixedUpdate(T data) {}
    public virtual void OnExit(T data) {}
    public virtual void OnSLateUpdate(T data) {}
    public virtual void OnSTriggerExit(T data, Collider other) {}
    public virtual void OnSCollisionEnter(T data, Collision other) {}
    public virtual void OnSTriggerEnter(T data, Collider other) {}
    public virtual void OnSCollisionExit(T data, Collision other) {}
    public virtual void OnSDrawGizmos(T data) {}
    public virtual void OnSDrawGizmosSelected(T data) {}
    public virtual void OnSDrawGizmosAlways(T data) {}

}
