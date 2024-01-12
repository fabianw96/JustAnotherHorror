using System;
using System.Collections;
using System.Collections.Generic;
using Interfaces;
using UnityEngine;
using UnityEngine.Serialization;

namespace Interactables
{
    public class Door : MonoBehaviour, IInteractable
    {
        private bool _isOpen;
        private Animator _animator;
        private readonly int _isOpenHash = Animator.StringToHash("isOpen");


        private void Start()
        {
            _animator = GetComponent<Animator>();
        }

        public void Interaction()
        {
            _isOpen = !_isOpen;
            _animator.SetBool(_isOpenHash, _isOpen);
        }
    }
}
