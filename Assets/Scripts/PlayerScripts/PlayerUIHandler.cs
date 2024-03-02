using System.Collections.Generic;
using Interfaces;
using Managers;
using PlayerScripts;
using UnityEngine;
using UnityEngine.UIElements;

namespace UI
{
    public class PlayerUIHandler : MonoBehaviour
    {
        public static bool StaminaBarEnabled;
        private VisualElement _hudRoot;
        private RaycastHit _raycastHit;
        private Dictionary<GameObject, IInteractable> _interactableCache = new();
        private LayerMask _hiddenLayer;

        [SerializeField] private GameObject interactHud;


        private void Start()
        {
            _hiddenLayer = LayerMask.NameToLayer("PlayerHidden");
            _hudRoot = interactHud.GetComponent<UIDocument>().rootVisualElement;
        }

        public void UpdateStaminaBar(float sprintTimer)
        {
            if (StaminaBarEnabled)
            {
                _hudRoot.Q<ProgressBar>("StaminaBar").value = sprintTimer;
                _hudRoot.Q<ProgressBar>("StaminaBar").visible = !GameManager.Instance.isPaused;
            }
            else
            {
                _hudRoot.Q<ProgressBar>("StaminaBar").visible = false;
            }
        }
        
        public void HighlightInteraction(Camera playerCamera)
        {
            if (playerCamera == null || !Physics.Raycast(playerCamera.ScreenPointToRay(Input.mousePosition),
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

            SetPlayerUIText();
        }

        private void SetPlayerUIText()
        {
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
    }
}
