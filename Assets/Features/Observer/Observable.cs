using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[System.Serializable]
public class Observable<T>
{
    private T _value;
    public T Value {
        get => _value;
        set {
            _value = value;
            OnValueChanged?.Invoke(value);
        }
    }

    public delegate void ValueChanged(T value);
    public ValueChanged OnValueChanged;

    public Observable(T value)
    {
        Value = value;
    }

    public void AddListener(ValueChanged listener)
    {
        OnValueChanged += listener;
    }

    public void RemoveListener(ValueChanged listener)
    {
        OnValueChanged -= listener;
    }

    public void RemoveAllListeners()
    {
        OnValueChanged = null;
    }

    public static implicit operator T(Observable<T> observable)
    {
        return observable.Value;
    }
}
