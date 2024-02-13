using System.Collections.Generic;
using Interfaces;
using Managers;
using UnityEngine;
using Random = UnityEngine.Random;


namespace Interactables
{
    public class Door : MonoBehaviour, IInteractable
    {
        private bool _isOpen;
        private bool _isUnlocked;
        private int _clipIndex;
        private readonly int _isOpenHash = Animator.StringToHash("isOpen");
        
        [SerializeField]private Animator animator;
        [SerializeField] private Key key;
        [SerializeField] private AudioSource audioSource;
        [SerializeField] private List<AudioClip> lockedDoorList;
        [SerializeField] private AudioClip unlockDoor;
        [SerializeField] private bool isFinalDoor;
        
        public void Interaction()
        {
            _clipIndex = Random.Range(0, lockedDoorList.Count);
            Debug.Log("Interacted with: " + gameObject);
            if (!GameplayManager.Instance.collectedKeys.Contains(key))
            {
                audioSource.clip = lockedDoorList[_clipIndex];
                audioSource.Play();
                return;
            }

            if (isFinalDoor)
            {
                GameplayManager.Instance.WinGame(true);
                return;
            }

            if (!_isUnlocked)
            {
                audioSource.PlayOneShot(unlockDoor);
                _isUnlocked = true;
            }
            
            _isOpen = !_isOpen;
            animator.SetBool(_isOpenHash, _isOpen);
        }

        public void OpenFinalDoor()
        {
            animator.SetBool(_isOpenHash, true);
        }
    }
}
