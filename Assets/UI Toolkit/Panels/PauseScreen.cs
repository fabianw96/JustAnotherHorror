using System;
using System.Collections;
using System.Collections.Generic;
using Managers;
using UnityEngine;
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
        root.Q<Button>("Settings").clicked += () => Debug.Log("Settings button clicked.");
        root.Q<Button>("Quit").clicked += () => Application.Quit();
    }
}
