using System;
using Interfaces;
using UnityEngine;

namespace Interactables
{
    public class HideLocker : MonoBehaviour, IInteractable
    {
        //When interaction is called from non-hidden state, lerp player camera to hidden position and disable collider.
        //when interaction is called from hidden state, move camera back in front of locker.
        [SerializeField] private GameObject player;
        [SerializeField] private Transform hideTransform;
        [SerializeField] private GameObject lockerParent;
        [SerializeField] private LayerMask hiddenLayer;
        [SerializeField] private LayerMask playerLayer;
        private Transform _lastPlayerPosition;
        private bool _isPlayerHidden = false;

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
            // _lastPlayerPosition = player.transform;
            Camera.main.transform.SetParent(hideTransform);
            Camera.main.transform.localPosition = Vector3.zero;
            player.layer = hiddenLayer;
            _isPlayerHidden = true;
        }

        private void UnhidePlayer()
        {
            player.SetActive(true);
            Camera.main.transform.SetParent(player.transform);
            Camera.main.transform.localPosition = new Vector3(0, 0.5f, 0);
            player.layer = playerLayer;
            _isPlayerHidden = false;
        }
    }
}
