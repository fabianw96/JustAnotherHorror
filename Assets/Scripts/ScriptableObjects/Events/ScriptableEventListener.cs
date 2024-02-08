using System;
using System.Collections;
using System.Collections.Generic;
using ScriptableObjects.Events;
using UnityEngine;
using UnityEngine.Events;

public class ScriptableEventListener : MonoBehaviour
{
    [SerializeField] private ScriptableEvent scriptableEvent;

    [SerializeField] private UnityEvent eventResponse;

    private void Awake()
    {
        scriptableEvent.Register(this);
    }

    private void OnDestroy()
    {
        scriptableEvent.UnRegister(this);
    }

    public void OnEventRaised()
    {
        eventResponse.Invoke();
    }
}
