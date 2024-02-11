using System;
using System.Collections;
using System.Collections.Generic;
using Interfaces;
using Managers;
using UnityEngine;

public class PaperNote : MonoBehaviour, IInteractable
{
    [SerializeField] private Canvas canvas;
    // [SerializeField] private Transform player;
    private Transform _cameraTrans;
    private bool _isReading;

    private void Start()
    {
        _cameraTrans = Camera.main.transform;
    }

    private void LateUpdate()
    {
        if (_isReading && Input.GetKeyDown(KeyCode.Space))
        {
            canvas.enabled = !canvas.enabled;
            _isReading = false;
            GameManager.Instance.PauseGame(false);
        }
        canvas.transform.forward = _cameraTrans.forward;
    }

    public void Interaction()
    {
        _isReading = true;
        canvas.enabled = !canvas.enabled;
        GameManager.Instance.PauseGame(false);
    }
}
