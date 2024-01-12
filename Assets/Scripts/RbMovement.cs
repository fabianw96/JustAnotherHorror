using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;

public class RbMovement : MonoBehaviour
{
    private Rigidbody _rBody;
    
    [Header("Movement")]
    private Vector2 _inputVector;
    private Vector3 _movementVector;
    [SerializeField] private float speed = 5f;
    [SerializeField] private float maxForce;

    [Header("Jump")]
    [SerializeField] private float jumpForce = 10f;
    
    [Header("Ground Check")]
    [SerializeField] private Transform groundCheck;
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private float groundCheckRadius;
    
    private void Awake()
    {
        _rBody = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        MoveCharacter();
    }
    
    
    public void OnMove(InputAction.CallbackContext context)
    {
        _inputVector = context.ReadValue<Vector2>();
    }
    
    public void OnJump(InputAction.CallbackContext context)
    {
        if (context.performed && IsGrounded())
        {
            HandleJump();
        }
    }
    
    private void MoveCharacter()
    {
        //find target velocity
        Vector3 currentVelocity = _rBody.velocity;
        Vector3 targetVelocity = new Vector3(_inputVector.x, 0f, _inputVector.y);
        
        targetVelocity *= speed;
        
        //align direction
        targetVelocity = transform.TransformDirection(targetVelocity);
        
        //calculate forces
        Vector3 velocityChange = (targetVelocity - currentVelocity);
        
        //limit force
        velocityChange = Vector3.ClampMagnitude(velocityChange, maxForce);

        _rBody.AddForce(new Vector3(velocityChange.x, 0f, velocityChange.z), ForceMode.VelocityChange);
        
    }

    private void HandleJump()
    {
        float jumpHeight = Mathf.Sqrt(-2f * jumpForce *  Physics.gravity.y);
        _rBody.AddForce(Vector3.up *  jumpHeight, ForceMode.Impulse);

    }

    private bool IsGrounded()
    {
        return Physics.CheckSphere(groundCheck.position, groundCheckRadius, groundLayer);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(groundCheck.position, groundCheckRadius);
    }
}
