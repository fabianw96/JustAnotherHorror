using Managers;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;
using Cursor = UnityEngine.Cursor;

namespace UI
{
    public class GameWonScreen : MonoBehaviour
    {
        private void Awake()
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            
            VisualElement root = GetComponent<UIDocument>().rootVisualElement;
            root.Q<Button>("MainMenu").clicked += () => SceneLoader.Load(SceneLoader.Scenes.MainMenu, LoadSceneMode.Single);
            root.Q<Button>("Quit").clicked += Application.Quit;
            
            #if UNITY_EDITOR
            root.Q<Button>("Quit").clicked += EditorQuitScript.QuitEditor;
            #endif      
        }
    }
}
