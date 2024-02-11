using System;
using Interactables;
using Interfaces;
using Managers;
using PlayerScripts;
using UnityEngine;
using UnityEngine.InputSystem;

namespace PlayerScripts
{
    [RequireComponent(typeof(Rigidbody))]
    public class PlayerHandler : MonoBehaviour
    {
        // private PlayerInteraction Interaction = new();
        private Rigidbody _rBody;
        private RaycastHit _raycastHit;
        [SerializeField] private GameObject interactHud;
    
        [Header("Movement")]
        private Vector2 _inputVector;
        private Vector3 _movementVector;
        private bool _isSprinting;
        [SerializeField] private float speed = 5f;
        [SerializeField] private float maxForce;
        [SerializeField] private float sprintMulti = 2f;
        private Camera myCamera;

        private void Start()
        {
            myCamera = Camera.main;
        }

        private void Awake()
        {
            _rBody = GetComponent<Rigidbody>();
        }

        private void Update()
        {
            HighlightInteraction();
        }

        // Update is called once per frame
        void FixedUpdate()
        {
            MoveCharacter();
        }
        
        private void HighlightInteraction()
        {
            if (myCamera == null || !Physics.Raycast(myCamera.ScreenPointToRay(Input.mousePosition), out _raycastHit, PlayerInteraction.RaycastDistance)) return;
            interactHud.SetActive(_raycastHit.transform.gameObject.GetComponent<IInteractable>() != null);
        }
    
    
        public void OnMove(InputAction.CallbackContext context)
        {
            _inputVector = context.ReadValue<Vector2>();
        }
        
        public void OnInteraction(InputAction.CallbackContext context)
        {
            if (context.performed && !GameManager.Instance.isPaused)
            {
                PlayerInteraction.Interact();
            }
        }

        public void OnSprint(InputAction.CallbackContext context)
        {
            _isSprinting = context.performed;
        }

        public void OnEscape(InputAction.CallbackContext context)
        {
            if (!GameManager.Instance.IsGameOver() && context.performed)
            {
                GameManager.Instance.PauseGame(true);
            }
        }
    
        private void MoveCharacter()
        {
            //find target velocity
            Vector3 currentVelocity = _rBody.velocity;
            Vector3 targetVelocity = new Vector3(_inputVector.x, 0f, _inputVector.y);

            if (_isSprinting)
            {
                targetVelocity *= speed * sprintMulti;
            }
            else
            {
                targetVelocity *= speed;
            }
            
            //align direction
            targetVelocity = transform.TransformDirection(targetVelocity);
        
            //calculate forces
            Vector3 velocityChange = (targetVelocity - currentVelocity);
        
            //limit force
            velocityChange = Vector3.ClampMagnitude(velocityChange, maxForce);

            _rBody.AddForce(new Vector3(velocityChange.x, 0f, velocityChange.z), ForceMode.VelocityChange);
        }
    }
}
