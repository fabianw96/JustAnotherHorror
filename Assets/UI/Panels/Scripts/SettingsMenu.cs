using System.Linq;
using Managers;
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
        private Slider _volumeSlider;
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
        }
    

        private void OnApply()
        {
            var resolution = Screen.resolutions[_displayResolution.index];
            Screen.SetResolution(resolution.width, resolution.height, true);
            QualitySettings.SetQualityLevel(_quality.index, true);
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

        private void InitQualitySettings()
        {
            _quality = _root.Q<DropdownField>("Quality");
            _quality.choices = QualitySettings.names.ToList();
            _quality.index = QualitySettings.GetQualityLevel();
        }

        private void InitDisplayResolution()
        {
            _displayResolution = _root.Q<DropdownField>("Resolution");
            _displayResolution.choices =
                Screen.resolutions.Select(resolution => $"{resolution.width}x{resolution.height}").ToList();
            _displayResolution.index = Screen.resolutions
                .Select((resolution, index) => (resolution, index))
                .First((value) => value.resolution.width == Screen.currentResolution.width &&
                                  value.resolution.height == Screen.currentResolution.height)
                .index;
        }
    }
}
