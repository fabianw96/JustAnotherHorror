using System;
using System.Collections;
using System.Collections.Generic;
using ScriptableObjects.Events;
using UnityEngine;
using UnityEngine.Serialization;

public class TriggerScript : MonoBehaviour
{
    [SerializeField] private ScriptableEvent triggerEvent;
    [SerializeField] private AudioSource audioSource;
    
    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<Rigidbody>() != null)
        {
            triggerEvent.RaiseEvent();
            audioSource.Play();
            this.gameObject.SetActive(false);
        }
    }
}
