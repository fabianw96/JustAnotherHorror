using System.Linq;
using Managers;
using PlayerScripts;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UIElements;

namespace UI.Panels.Scripts
{
    public class SettingsMenu : MonoBehaviour
    {
        [SerializeField] private AudioMixer audioMixer;
        private DropdownField _displayResolution;
        private DropdownField _quality;
        private Toggle _fullscreenToggle;
        private Toggle _staminaBarToggle;
        private Slider _volumeSlider;
        private Slider _sensSlider;
        private VisualElement _root;
        private float _currentVolume;
    
        private void OnEnable()
        {
            _root = GetComponent<UIDocument>().rootVisualElement;
            
            _root.Q<Button>("Save").clicked += OnApply;
            _root.Q<Button>("Cancel").clicked += OnCancel;
            InitDisplayResolution();
            InitQualitySettings();
            InitVolume();
            InitSens();
            InitShowStaminaBar();
        }
    

        private void OnApply()
        {
            var resolution = Screen.resolutions[_displayResolution.index];
            Screen.SetResolution(resolution.width, resolution.height, _fullscreenToggle.value);
            QualitySettings.SetQualityLevel(_quality.index, true);
            PlayerCamera.SetMouseSens(_sensSlider.value);
            PlayerUIHandler.StaminaBarEnabled = _staminaBarToggle.value;
            audioMixer.SetFloat("Volume", Mathf.Log10(_volumeSlider.value) * 20);
            GameManager.Instance.isExtraMenu = false;
            GameManager.Instance.SwitchMenu();
        }

        private void OnCancel()
        {
            GameManager.Instance.isExtraMenu = false;
            GameManager.Instance.SwitchMenu();
        }

        private void InitVolume()
        {
            _volumeSlider = _root.Q<Slider>("Volume");
            audioMixer.GetFloat("Volume", out _currentVolume);
            _volumeSlider.value = Mathf.Pow(10, (_currentVolume / 20));
        }

        private void InitSens()
        {
            _sensSlider = _root.Q<Slider>("Sens");
            _sensSlider.value = PlayerCamera.GetMouseSens();
        }

        private void InitQualitySettings()
        {
            _quality = _root.Q<DropdownField>("Quality");
            _quality.choices = QualitySettings.names.ToList();
            _quality.index = QualitySettings.GetQualityLevel();
        }

        private void InitDisplayResolution()
        {
            _fullscreenToggle = _root.Q<Toggle>("Fullscreen");
            _fullscreenToggle.value = Screen.fullScreen;
            _displayResolution = _root.Q<DropdownField>("Resolution");
            _displayResolution.choices =
                Screen.resolutions.Select(resolution => $"{resolution.width}x{resolution.height}").ToList();
            _displayResolution.index = Screen.resolutions
                .Select((resolution, index) => (resolution, index))
                .First((value) => value.resolution.width == Screen.currentResolution.width &&
                                  value.resolution.height == Screen.currentResolution.height)
                .index;
        }

        private void InitShowStaminaBar()
        {
            _staminaBarToggle = _root.Q<Toggle>("StaminaBarToggle");
            _staminaBarToggle.value = PlayerUIHandler.StaminaBarEnabled;
        }
    }
}
