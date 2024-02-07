using System;
using Managers;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

namespace UI_Toolkit.Panels
{
    public class MainMenuPresenter : MonoBehaviour
    {
        private void OnEnable()
        {
            // Time.timeScale = 0f;
            VisualElement root = GetComponent<UIDocument>().rootVisualElement;
            root.Q<Button>("Start").clicked += () => SceneLoader.Load(SceneLoader.Scenes.SampleScene, LoadSceneMode.Single);
            root.Q<Button>("Settings").clicked += OpenSettings;
            root.Q<Button>("Quit").clicked += () => Application.Quit();
        }

        private void OpenSettings()
        {
            GameManager.Instance.SwitchMenu();
        }
    }
}
