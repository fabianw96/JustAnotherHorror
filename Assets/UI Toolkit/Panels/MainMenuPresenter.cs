using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

namespace UI_Toolkit.Panels
{
    public class MainMenuPresenter : MonoBehaviour
    {
        private void Awake()
        {
            // Time.timeScale = 0f;
            VisualElement root = GetComponent<UIDocument>().rootVisualElement;
            root.Q<Button>("Start").clicked += () => SceneLoader.Load(SceneLoader.Scenes.SampleScene);
            root.Q<Button>("Settings").clicked += () => Debug.Log("Settings button clicked.");
            root.Q<Button>("Quit").clicked += () => Application.Quit();
        }
    }
}
