using System;
using System.Collections.Generic;
using UnityEngine;

namespace Managers
{
    public class GameManager : MonoBehaviour
    {
        public static GameManager Instance;
        private List<GameObject> interactableList;
        private bool _isGameOver;
        
        private void Awake()
        {
            if (Instance != null && Instance != this)    
            {
                Destroy(this);
            }
            else
            {
                Instance = this;
            }
            DontDestroyOnLoad(this);
        }


        private void Update()
        {
            if (_isGameOver)
            {
                
            }
        }

        public void GameOver(bool state)
        {
            _isGameOver = state;
        }

        public bool IsGameOver()
        {
            return _isGameOver;
        }
        
    }
}