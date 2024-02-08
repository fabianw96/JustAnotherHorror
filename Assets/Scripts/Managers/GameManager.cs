using System;
using System.Collections.Generic;
using ScriptableObjects.Events;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Managers
{
    public class GameManager : MonoBehaviour
    {
        [SerializeField] private GameObject pausePrefab;
        [SerializeField] private GameObject settingsPrefab;
        [SerializeField] private GameObject mainMenuPrefab;
        [SerializeField] private ScriptableEvent playerDeathEvent;

        public static GameManager Instance;
        private List<GameObject> interactableList;
        private bool _isGameOver;
        public bool isPaused;
        private GameObject pScreen;
        private GameObject sScreen;
        private GameObject mScreen;
        
        
        private void Awake()
        {
            //singleton
            if (Instance != null && Instance != this)    
            {
                Destroy(this);
                return;
            }
            Instance = this;
            DontDestroyOnLoad(this);

            //call when a scene is loaded
            SceneManager.sceneLoaded += OnSceneLoaded;
        }

        private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
        {
            //only load Main menu and settings when actually in main menu
            if (scene.name == SceneManager.GetSceneByBuildIndex(0).name)
            {
                mScreen = Instantiate(mainMenuPrefab);
                mScreen.SetActive(true);
                sScreen = Instantiate(settingsPrefab);
                sScreen.SetActive(false);
            }

            //load settings and pause menu when in game scene
            if (scene.name == SceneManager.GetSceneByBuildIndex(1).name)
            {
                sScreen = Instantiate(settingsPrefab);
                sScreen.SetActive(false);
                pScreen = Instantiate(pausePrefab);
                pScreen.SetActive(false);
            }
        }
        

        public void GameOver(bool state)
        {
            _isGameOver = state;

            if (_isGameOver)
            {
                playerDeathEvent.RaiseEvent();
            }
        }

        public void PauseGame()
        {
            isPaused = !isPaused;
            if (isPaused)
            {
                pScreen.SetActive(true);
                Cursor.lockState = CursorLockMode.Confined;
                Cursor.visible = true;
            }
            else if (!isPaused)
            {
                pScreen.SetActive(false);
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;
            }
            Time.timeScale = isPaused ? 0 : 1;
        }

        public void SwitchMenu()
        {
            //check for null before setting the gameobject to active/inactive
            if (mScreen != null)
            {
                mScreen.SetActive(!mScreen.activeSelf);
            }
            
            if (pScreen != null)
            {
                pScreen.SetActive(!pScreen.activeSelf);
            }
            
            if (sScreen != null)
            {
                sScreen.SetActive(!sScreen.activeSelf);
            }
        }
        
        public bool IsGameOver()
        {
            return _isGameOver;
        }
        
    }
}