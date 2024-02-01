using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Managers;
using UnityEngine;
using UnityEngine.InputSystem.HID;
using UnityEngine.UIElements;
using Cursor = UnityEngine.Cursor;

public class SettingsMenu : MonoBehaviour
{
    private DropdownField _displayResolution;
    private DropdownField _quality;

    private VisualElement root;
    private void Awake()
    {
        root = GetComponent<UIDocument>().rootVisualElement;

        root.Q<Button>("Save").clicked += () => OnApply();
        root.Q<Button>("Cancel").clicked += () => SceneLoader.Unload(SceneLoader.Scenes.SettingsMenu);
        InitDisplayResolution();
        InitQualitySettings();
    }

    private void OnApply()
    {
        var resolution = Screen.resolutions[_displayResolution.index];
        Screen.SetResolution(resolution.width, resolution.height, true);
        QualitySettings.SetQualityLevel(_quality.index, true);
        SceneLoader.Unload(SceneLoader.Scenes.SettingsMenu);
    }

    private void InitQualitySettings()
    {
        _quality = root.Q<DropdownField>("Quality");
        _quality.choices = QualitySettings.names.ToList();
        _quality.index = QualitySettings.GetQualityLevel();
    }

    private void InitDisplayResolution()
    {
        _displayResolution = root.Q<DropdownField>("Resolution");
        _displayResolution.choices =
            Screen.resolutions.Select(resolution => $"{resolution.width}x{resolution.height}").ToList();
        _displayResolution.index = Screen.resolutions
            .Select((resolution, index) => (resolution, index))
            .First((value) => value.resolution.width == Screen.currentResolution.width &&
                                                       value.resolution.height == Screen.currentResolution.height)
            .index;
    }
}
