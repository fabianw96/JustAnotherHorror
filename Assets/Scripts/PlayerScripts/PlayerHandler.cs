using System.Collections;
using System.Collections.Generic;
using Interfaces;
using Managers;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;

namespace PlayerScripts
{
    [RequireComponent(typeof(Rigidbody))]
    public class PlayerHandler : MonoBehaviour
    {
        private Rigidbody _rBody;
        private RaycastHit _raycastHit;
        private VisualElement _hudRoot;
        private LayerMask _hiddenLayer;
        [SerializeField] private GameObject interactHud;

    
        [Header("Movement")]
        private Vector2 _inputVector;
        private Vector3 _movementVector;
        private bool _isSprinting;
        private bool _canSprint = true;
        private float _sprintTimer;
        [SerializeField] private float maxSprintTime = 2f;
        [SerializeField] private float speed = 5f;
        [SerializeField] private float maxForce;
        [SerializeField] private float sprintMulti = 2f;
        private Camera _myCamera;
        private Dictionary<GameObject, IInteractable> _interactableCache = new();
        
        
        private void Start()
        {
            _myCamera = Camera.main;
            _sprintTimer = maxSprintTime;
            _hiddenLayer = LayerMask.NameToLayer("PlayerHidden");
            _hudRoot = interactHud.GetComponent<UIDocument>().rootVisualElement;
        }

        private void Awake()
        {
            _rBody = GetComponent<Rigidbody>();
        }

        private void Update()
        {
            HighlightInteraction();
            _hudRoot.Q<ProgressBar>("StaminaBar").value = _sprintTimer;
            _hudRoot.Q<ProgressBar>("StaminaBar").visible = !GameManager.Instance.isPaused;
            
            if (_sprintTimer <= 0f)
            {
                _canSprint = false;
                _sprintTimer = 0f;
            }
        }

        // Update is called once per frame
        void FixedUpdate()
        {
            MoveCharacter();
        }
        
        private void HighlightInteraction()
        {
            if (_myCamera == null || !Physics.Raycast(_myCamera.ScreenPointToRay(Input.mousePosition),
                    out _raycastHit,
                    PlayerInteraction.RaycastDistance))
            {
                _hudRoot.Q<Label>("Interact").visible = false;
                _hudRoot.Q<Label>("InteractLabel").visible = false;
                return;
            }

            if (!_interactableCache.ContainsKey(_raycastHit.transform.gameObject))
            {
                _interactableCache[_raycastHit.transform.gameObject] = _raycastHit.transform.gameObject.GetComponent<IInteractable>();
                Debug.Log("Added Interactable to dictionary: " + _interactableCache[_raycastHit.transform.gameObject]);
            }
            
            _hudRoot.Q<Label>("Interact").visible = _interactableCache[_raycastHit.transform.gameObject] != null;
            _hudRoot.Q<Label>("InteractLabel").visible = _interactableCache[_raycastHit.transform.gameObject] != null;


            switch (_raycastHit.transform.gameObject.tag)
            {
                case "Locker":
                    if (gameObject.layer == _hiddenLayer)
                    {
                        _hudRoot.Q<Label>("InteractLabel").text = "LMB to unhide";
                        break;
                    }
                    _hudRoot.Q<Label>("InteractLabel").text = "LMB to hide";
                    break;
                case "Key":
                    _hudRoot.Q<Label>("InteractLabel").text = "LMB to pickup key";
                    break;
                case "Door":
                    _hudRoot.Q<Label>("InteractLabel").text = "LMB to open door";
                    break;
                case "Note":
                    _hudRoot.Q<Label>("InteractLabel").text = "LMB to pickup note";
                    break;
                default:
                    _hudRoot.Q<Label>("InteractLabel").text = "LMB to interact";
                    break;
            }
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
            if (!GameplayManager.Instance.IsGameOver() && context.performed)
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
