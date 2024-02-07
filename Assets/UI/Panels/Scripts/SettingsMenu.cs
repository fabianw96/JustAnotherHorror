using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Managers;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.InputSystem.HID;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;
using Cursor = UnityEngine.Cursor;

public class SettingsMenu : MonoBehaviour
{
    [SerializeField] private AudioMixer audioMixer;
    private DropdownField _displayResolution;
    private DropdownField _quality;
    private Slider _volumeSlider;
    private VisualElement root;
    
    private void OnEnable()
    {
        root = GetComponent<UIDocument>().rootVisualElement;

        root.Q<Button>("Save").clicked += OnApply;
        root.Q<Button>("Cancel").clicked += OnCancel;
        InitDisplayResolution();
        InitQualitySettings();
        InitVolume();
    }
    

    private void OnApply()
    {
        var resolution = Screen.resolutions[_displayResolution.index];
        Screen.SetResolution(resolution.width, resolution.height, true);
        QualitySettings.SetQualityLevel(_quality.index, true);
        audioMixer.SetFloat("Volume", Mathf.Log10(_volumeSlider.value) * 20);
        GameManager.Instance.SwitchMenu();
    }

    private void OnCancel()
    {
        GameManager.Instance.SwitchMenu();
    }

    private void InitVolume()
    {
        _volumeSlider = root.Q<Slider>("Volume");
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
