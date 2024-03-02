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
        [SerializeField] private int playerLives = 2;
        public static GameplayManager Instance;
        public List<Key> collectedKeys;
        private bool _isGameOver;
        private bool _hasEnded;
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
            if (!_hasWon) return;
            _endTimer -= Time.deltaTime;
            
            if (!(_endTimer <= 0)) return;
            
            _endTimer = 0;
            Debug.Log("You won!");
            playerWinEvent.RaiseEvent();
            _hasWon = false;
        }

        public void AddKeyToList(Key key)
        {
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
        
        public bool HasEnded()
        {
            return _hasEnded;
        }
    }
}