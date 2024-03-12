using Managers;
using UnityEngine;
using UnityEngine.UIElements;

namespace UI
{
    public class PauseScreen : MonoBehaviour
    {
        private void OnEnable()
        {
            //get UIdocument and call methods based on button
            VisualElement root = GetComponent<UIDocument>().rootVisualElement;
            root.Q<Button>("Return").clicked += () => GameManager.Instance.PauseGame(false);
            root.Q<Button>("Settings").clicked += OpenSettings;
            root.Q<Button>("Quit").clicked += Application.Quit;
            
            //TODO: remove static "/4", variable with how many keys are in level.
            root.Q<Label>("CollectedKeyCount").text = GameplayManager.Instance.collectedKeys.Count + "/4";
            
            #if UNITY_EDITOR
            root.Q<Button>("Quit").clicked += EditorQuitScript.QuitEditor;
            #endif
        }

        private void OpenSettings()
        {
            //switch between current menus
            GameManager.Instance.isExtraMenu = true;
            GameManager.Instance.SwitchMenu();
        }
    }
}
