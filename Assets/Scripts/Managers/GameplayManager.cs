using System.Collections.Generic;
using Interactables;
using ScriptableObjects.Events;
using UnityEngine;

namespace Managers
{
    public class GameplayManager : MonoBehaviour
    {
        [SerializeField] private ScriptableEvent playerDeathEvent;
        [SerializeField] private ScriptableEvent playerWinEvent;
        public static GameplayManager Instance;
        public List<Key> collectedKeys;
        private bool _isGameOver;
        private bool _hasWon;
        private float _endTimer = 5f;

        private void Awake()
        {
            if (Instance != null && Instance != this)    
            {
                Destroy(gameObject);
                return;
            }
            Instance = this;
        }

        private void Update()
        {
            //check if player has touched final door with key
            if (!_hasWon) return;
            _endTimer -= Time.deltaTime;
            
            //
            if (!(_endTimer <= 0)) return;
            
            _endTimer = 0;
            Debug.Log("Can escape");
            
            //opens last door and spawns wingame trigger
            playerWinEvent.RaiseEvent();
            
            //reset has won bool
            _hasWon = false;
        }

        public void AddKeyToList(Key key)
        {
            //adds collected key to the list for doors to check
            collectedKeys.Add(key);
        }
        
        public void GameOver(bool state)
        {
            _isGameOver = state;

            if (_isGameOver)
            {
                playerDeathEvent.RaiseEvent();
            }
        }
        
        public bool IsGameOver()
        {
            return _isGameOver;
        }

        public void WinGame(bool state)
        {
            _hasWon = state;
        }
    }
}