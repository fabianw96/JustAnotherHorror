using System.Collections;
using System.Collections.Generic;
using Interfaces;
using Managers;
using UI;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;
using UnityEngine.UIElements;

namespace PlayerScripts
{
    [RequireComponent(typeof(Rigidbody))]
    public class PlayerHandler : MonoBehaviour
    {
        private Rigidbody _rBody;
        private VisualElement _hudRoot;
        [SerializeField] private AudioClip breathClip;
        
        [Header("UI")]
        [SerializeField] private PlayerUIHandler uiHandler;

    
        [Header("Movement")]
        private Vector2 _inputVector;
        private Vector3 _movementVector;
        private bool _isSprinting;
        private bool _canSprint = true;
        private float _sprintTimer;
        private Camera _myCamera;
        [SerializeField] private float maxSprintTime = 2f;
        [SerializeField] private float speed = 5f;
        [SerializeField] private float maxForce;
        [SerializeField] private float sprintMulti = 2f;
        
        
        private void Start()
        {
            _myCamera = Camera.main;
            _sprintTimer = maxSprintTime;
        }

        private void Awake()
        {
            _rBody = GetComponent<Rigidbody>();
        }

        private void Update()
        {
            uiHandler.HighlightInteraction(_myCamera);
            uiHandler.UpdateStaminaBar(_sprintTimer);
            
            if (_sprintTimer <= 0f)
            {
                _canSprint = false;
                _sprintTimer = 0f;
                AudioManager.Instance.playerAudio.PlayOneShot(breathClip);
            }
        }

        // Update is called once per frame
        private void FixedUpdate()
        {
            MoveCharacter();
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
            if (!GameplayManager.Instance.IsGameOver() && context.performed && !GameManager.Instance.isExtraMenu)
            {
                GameManager.Instance.PauseGame(true);
            }
        }
    
        private void MoveCharacter()
        {
            //find target velocity
            Vector3 currentVelocity = _rBody.velocity;
            Vector3 targetVelocity = new Vector3(_inputVector.x, 0f, _inputVector.y);

            if (_isSprinting && _canSprint)
            {
                _sprintTimer -= Time.deltaTime;
                targetVelocity *= speed * sprintMulti;
            }
            else
            {
                if (_sprintTimer <= maxSprintTime)
                {
                    _sprintTimer += Time.deltaTime;
                }
                
                if (_sprintTimer >= maxSprintTime)
                {
                    _sprintTimer = maxSprintTime;
                    _canSprint = true;
                }
                
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
