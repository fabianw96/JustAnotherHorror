using System;
using Interfaces;
using Managers;
using UnityEngine;


namespace Interactables
{
    public class Door : MonoBehaviour, IInteractable
    {
        private bool _isOpen;
        [SerializeField]private Animator animator;
        private readonly int _isOpenHash = Animator.StringToHash("isOpen");
        [SerializeField] private Key key;
        
        public void Interaction()
        {
            Debug.Log("Interacted with: " + gameObject);
            if (!GameplayManager.instance.collectedKeys.Contains(key)) return;
            _isOpen = !_isOpen;
            animator.SetBool(_isOpenHash, _isOpen);
        }
    }
}
