using System;
using System.Collections;
using System.Collections.Generic;
using Managers;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;
using Cursor = UnityEngine.Cursor;

public class PauseScreen : MonoBehaviour
{
    private void Awake()
    {
        Cursor.lockState = CursorLockMode.Confined;
        Cursor.visible = true;
        
        VisualElement root = GetComponent<UIDocument>().rootVisualElement;
        root.Q<Button>("Return").clicked += () => GameManager.Instance.PauseGame();
        root.Q<Button>("Settings").clicked +=
            () => SceneLoader.Load(SceneLoader.Scenes.SettingsMenu, LoadSceneMode.Additive);
        root.Q<Button>("Quit").clicked += () => Application.Quit();
    }
}
