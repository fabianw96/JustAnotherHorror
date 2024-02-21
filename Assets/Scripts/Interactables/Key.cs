using UnityEngine;
using Interfaces;
using Managers;
using ScriptableObjects.Events;

namespace Interactables
{
    public class Key : MonoBehaviour, IInteractable
    {
        // [SerializeField] private int keyIdentifier;
        [SerializeField] private ScriptableEvent triggeredEvent;
        public void Interaction()
        {
            GameplayManager.Instance.AddKeyToList(this);
            triggeredEvent.RaiseEvent();
            gameObject.SetActive(false);
            // Destroy(gameObject);
        }
    }
}
