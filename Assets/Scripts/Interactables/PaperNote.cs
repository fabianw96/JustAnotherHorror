using System;
using System.Collections;
using System.Collections.Generic;
using Interfaces;
using UnityEngine;

public class PaperNote : MonoBehaviour, IInteractable
{
    [SerializeField] private Canvas canvas;
    // [SerializeField] private Transform player;
    private Transform cameraTrans;

    private void Start()
    {
        cameraTrans = Camera.main.transform;
    }

    private void LateUpdate()
    {
        canvas.transform.forward = cameraTrans.forward;
    }

    public void Interaction()
    {
        canvas.enabled = !canvas.enabled;
    }
}
