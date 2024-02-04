using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Managers
{
    public class GameManager : MonoBehaviour
    {
        [SerializeField] private GameObject pauseScreen;

        public static GameManager Instance;
        private List<GameObject> interactableList;
        private bool _isGameOver;
        public bool isPaused;
        // private GameObject pScreen;
        // private GameObject goScreen;
        
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
        

        public void GameOver(bool state)
        {
            _isGameOver = state;
        }

        public void PauseGame()
        {
            isPaused = !isPaused;
            if (isPaused)
            {
                SceneLoader.Load(SceneLoader.Scenes.PauseMenu, LoadSceneMode.Additive);
            }
            else if (!isPaused)
            {
                SceneLoader.Unload(SceneLoader.Scenes.PauseMenu);
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;
            }
            Time.timeScale = isPaused ? 0 : 1;
        }
        
        public bool IsGameOver()
        {
            return _isGameOver;
        }
        
    }
}