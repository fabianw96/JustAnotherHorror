using UnityEngine;
using Interfaces;
using Managers;

namespace Interactables
{
    public class Key : MonoBehaviour, IInteractable
    {
        [SerializeField] private int keyIdentifier;
        
        public void Interaction()
        {
            GameplayManager.instance.AddKeyToList(this);
            gameObject.SetActive(false);
        }
    }
}
