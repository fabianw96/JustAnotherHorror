using UnityEngine;
using UnityEngine.InputSystem;
using Interfaces;

namespace PlayerScripts
{
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
        
        [Header("Raycast")]
        [SerializeField] private float raycastDistance = 5f;

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
        
        
        public void OnInteraction(InputAction.CallbackContext context)
        {
            if (context.performed)
            {
                CastRay();
            }
        }
    
        private void CastRay()
        {
            RaycastHit hitInfo = new RaycastHit();
            bool hit = Camera.main != null && Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hitInfo, raycastDistance);

            if (!hit) return;
            GameObject hitObject = hitInfo.transform.gameObject;
            if (hitObject != null && hitObject.GetComponent<IInteractable>() != null)
            {
                hitObject.GetComponent<IInteractable>().Interaction();
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
}
