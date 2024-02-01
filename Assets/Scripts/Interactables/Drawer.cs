using System;
using UnityEngine;
using Interfaces;

namespace Interactables
{
    public class Drawer : MonoBehaviour, IInteractable
    {
        private bool _isOpen;
        [SerializeField] private Animator animator;
        private readonly int _isOpenHash = Animator.StringToHash("isOpen");
        
        public void Interaction()
        {
            Debug.Log("Interacted with: " + this);
            _isOpen = !_isOpen;
            animator.SetBool(_isOpenHash, _isOpen);
        }
    }
}

