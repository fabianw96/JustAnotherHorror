using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

namespace UI_Toolkit.Panels
{
    public class GameOverScreen : MonoBehaviour
    {
        private void Awake()
        {
            VisualElement root = GetComponent<UIDocument>().rootVisualElement;
            root.Q<Button>("Restart").clicked += () => SceneManager.LoadScene(SceneManager.GetActiveScene().name);
            root.Q<Button>("Quit").clicked += Application.Quit;
        }
    }
}
