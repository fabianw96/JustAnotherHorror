using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;

public class PlayerCamera : MonoBehaviour
{
    [SerializeField] private Transform playerBody;
    [SerializeField] private float mouseSens = 50f;
    private float _mouseInputX;
    private float _mouseInputY;
    private float _xRotation;

    

    // Start is called before the first frame update
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    // Update is called once per frame
    void LateUpdate()
    {
        Vector3 playerRotation = Vector3.up * (_mouseInputX * mouseSens);
        playerBody.Rotate(playerRotation);
        
        _xRotation -= _mouseInputY * mouseSens;
        _xRotation = Mathf.Clamp(_xRotation, -90, 90);
        
        transform.localRotation = Quaternion.Euler(_xRotation, 0f, 0f);
    }

    public void OnLook(InputAction.CallbackContext context)
    {
        _mouseInputX = context.ReadValue<Vector2>().x;
        _mouseInputY = context.ReadValue<Vector2>().y;
    }
}
