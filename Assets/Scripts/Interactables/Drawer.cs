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
                    audioSource.clip = drawerSounds[0];
                    break;
                case true:
                    audioSource.clip = drawerSounds[1];
                    break;
            }
            audioSource.Play();
            animator.SetBool(_isOpenHash, _isOpen);
        }
    }
}

