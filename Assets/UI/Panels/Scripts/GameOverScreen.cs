using System;
using Managers;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;
using Cursor = UnityEngine.Cursor;

namespace UI_Toolkit.Panels
{
    public class GameOverScreen : MonoBehaviour
    {
        private void Awake()
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            
            VisualElement root = GetComponent<UIDocument>().rootVisualElement;
            root.Q<Button>("MainMenu").clicked += () => MainMenu();
            root.Q<Button>("Quit").clicked += Application.Quit;
        }

        private void MainMenu()
        {
            Debug.Log("Test!");
            // GameManager.Instance.GameOver(false);
            GameplayManager.Instance.GameOver(false);
            SceneLoader.Load(SceneLoader.Scenes.MainMenu, LoadSceneMode.Single);
        }
    }
}
