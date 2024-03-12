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
        [SerializeField] private FinalDoor finalDoor;
        [SerializeField] private AudioSource audioSource;
        [SerializeField] private List<AudioClip> doorSoundsList;
        [SerializeField] private AudioClip unlockDoor;
        [SerializeField] private AudioClip openDoor;
        [SerializeField] private bool isFinalDoor;
        
        
        
        public void Interaction()
        {
            _clipIndex = Random.Range(0, doorSoundsList.Count);
            Debug.Log("Interacted with: " + gameObject);
            //if key has not been picked up, play random "rattle" sound
            if (!GameplayManager.Instance.collectedKeys.Contains(key))
            {
                audioSource.clip = doorSoundsList[_clipIndex];
                audioSource.Play();
                return;
            }

            //final door triggers special event
            if (isFinalDoor)
            {
                finalDoor.OnFinalStand();
                GameplayManager.Instance.WinGame(true);
                return;
            }
            
            if (!_isUnlocked)
            {
                audioSource.PlayOneShot(unlockDoor);
                _isUnlocked = true;
                return;
            }
            
            if (_isUnlocked && !_isOpen)
            {
                audioSource.clip = openDoor;
            }
            
            audioSource.Play();
            _isOpen = true;
            OpenDoor(_isOpen);
        }

        public void OpenDoor(bool state)
        {
            animator.SetBool(_isOpenHash, state);
        }

        public void OpenFinalDoor()
        {
            audioSource.PlayOneShot(unlockDoor);
            animator.SetBool(_isOpenHash, true);
            audioSource.PlayOneShot(openDoor);
        }
    }
}
