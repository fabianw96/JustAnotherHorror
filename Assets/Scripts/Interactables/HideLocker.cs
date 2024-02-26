using System;
using System.Collections.Generic;
using Interfaces;
using UnityEngine;
using UnityEngine.Serialization;

namespace Interactables
{
    public class HideLocker : MonoBehaviour, IInteractable
    {
        private LayerMask _hiddenLayer;
        private LayerMask _playerLayer;
        private bool _isPlayerHidden = false;
        
        //When interaction is called from non-hidden state, lerp player camera to hidden position and disable collider.
        //when interaction is called from hidden state, move camera back in front of locker.
        [SerializeField] private GameObject camHolder;
        [SerializeField] private GameObject player;
        [SerializeField] private Transform hideTransform;
        [SerializeField] private List<AudioClip> clipList;
        [SerializeField] private AudioSource audioSource;
        [SerializeField] private Light hideLight;
        [SerializeField] private Light playerLight;


        private void Awake()
        {
            _hiddenLayer = LayerMask.NameToLayer("PlayerHidden");
            _playerLayer = LayerMask.NameToLayer("Player");
        }
        
        public void Interaction()
        {
            if (!_isPlayerHidden)
            {
                HidePlayer();
            }
            else if (_isPlayerHidden)
            {
                UnhidePlayer();
            }
        }

        private void HidePlayer()
        {
            audioSource.PlayOneShot(clipList[0]);
            Camera.main.transform.SetParent(hideTransform);
            Camera.main.transform.localPosition = Vector3.zero;
            Camera.main.transform.localRotation = Quaternion.identity;
            hideLight.enabled = true;
            playerLight.enabled = false;
            player.layer = _hiddenLayer;
            _isPlayerHidden = true;
        }

        private void UnhidePlayer()
        {
            audioSource.PlayOneShot(clipList[1]);
            Camera.main.transform.SetParent(camHolder.transform);
            Camera.main.transform.localPosition = new Vector3(0, 0.5f, 0);
            Camera.main.transform.localRotation = Quaternion.identity;
            hideLight.enabled = false;
            playerLight.enabled = true;
            player.layer = _playerLayer;
            _isPlayerHidden = false;
        }
    }
}
