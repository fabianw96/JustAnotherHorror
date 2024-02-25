using Managers;
using UnityEngine;
using UnityEngine.UIElements;

namespace UI.Panels.Scripts
{
    public class PauseScreen : MonoBehaviour
    {
        private void OnEnable()
        {
            VisualElement root = GetComponent<UIDocument>().rootVisualElement;
            root.Q<Button>("Return").clicked += () => GameManager.Instance.PauseGame(false);
            root.Q<Button>("Settings").clicked += OpenSettings;
            root.Q<Button>("Quit").clicked += Application.Quit;
        }

        private void OpenSettings()
        {
            GameManager.Instance.isExtraMenu = true;
            GameManager.Instance.SwitchMenu();
        }
    }
}
