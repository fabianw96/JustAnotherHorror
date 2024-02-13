using System.Collections.Generic;
using UnityEngine;
using Interfaces;

namespace Interactables
{
    public class Drawer : MonoBehaviour, IInteractable
    {
        private bool _isOpen;
        private readonly int _isOpenHash = Animator.StringToHash("isOpen");

        [SerializeField] private Animator animator;
        [SerializeField] private List<AudioClip> drawerSounds;
        [SerializeField] private AudioSource audioSource;
        
        public void Interaction()
        {
            Debug.Log("Interacted with: " + this);
            _isOpen = !_isOpen;
            switch (_isOpen)
            {
                case false:
                    audioSource.PlayOneShot(drawerSounds[0]);
                    break;
                case true:
                    audioSource.PlayOneShot(drawerSounds[1]);
                    break;
            }
            animator.SetBool(_isOpenHash, _isOpen);
        }
    }
}

