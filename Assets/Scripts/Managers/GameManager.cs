using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Managers
{
    public class GameManager : MonoBehaviour
    {
        [SerializeField] private GameObject pausePrefab;
        [SerializeField] private GameObject settingsPrefab;
        [SerializeField] private GameObject mainMenuPrefab;

        public static GameManager Instance;
        private List<GameObject> _interactableList;
        private bool _isGameOver;
        public bool isPaused;
        private GameObject _pScreen;
        private GameObject _sScreen;
        private GameObject _mScreen;
        
        
        private void Awake()
        {
            //singleton
            if (Instance != null && Instance != this)    
            {
                Destroy(gameObject);
                return;
            }
            Instance = this;
            DontDestroyOnLoad(gameObject);

            //call when a scene is loaded
            SceneManager.sceneLoaded += OnSceneLoaded;
        }

        private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
        {
            //only load Main menu and settings when actually in main menu
            if (scene.name == SceneManager.GetSceneByBuildIndex(0).name)
            {
                _mScreen = Instantiate(mainMenuPrefab);
                _mScreen.SetActive(true);
                _sScreen = Instantiate(settingsPrefab);
                _sScreen.SetActive(false);
            }

            //load settings and pause menu when in game scene
            if (scene.name == SceneManager.GetSceneByBuildIndex(1).name)
            {
                _sScreen = Instantiate(settingsPrefab);
                _sScreen.SetActive(false);
                _pScreen = Instantiate(pausePrefab);
                _pScreen.SetActive(false);
            }
        }

        public void PauseGame(bool pressedEscape)
        {
            isPaused = !isPaused;
            switch (isPaused)
            {
                case true:
                    if (pressedEscape)
                    {
                        _pScreen.SetActive(true);
                        Cursor.lockState = CursorLockMode.Confined;
                        Cursor.visible = true;
                    }
                    break;
                case false:
                    _pScreen.SetActive(false);
                    Cursor.lockState = CursorLockMode.Locked;
                    Cursor.visible = false;
                    break;
            }
            Time.timeScale = isPaused ? 0 : 1;
        }

        public void SwitchMenu()
        {
            //check for null before setting the gameobject to active/inactive
            if (_mScreen != null)
            {
                _mScreen.SetActive(!_mScreen.activeSelf);
            }
            
            if (_pScreen != null)
            {
                _pScreen.SetActive(!_pScreen.activeSelf);
            }
            
            if (_sScreen != null)
            {
                _sScreen.SetActive(!_sScreen.activeSelf);
            }
        }
    }
}