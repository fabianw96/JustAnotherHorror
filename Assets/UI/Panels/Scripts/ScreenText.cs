using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UIElements;

namespace UI.Panels.Scripts
{
    public class ScreenText : MonoBehaviour
    {
        private Label _label;
        private float _timeElapsed;
        
        [SerializeField]private float timeOnScreen = 5f;

    
        private void Awake()
        { 
            _label = GetComponent<UIDocument>().rootVisualElement.Q<Label>();
            _timeElapsed = timeOnScreen;
        }

        private void Update()
        {
            if (_timeElapsed <= 0)
            {
                _label.text = "";
                _timeElapsed = 0f;
            }
            
            _timeElapsed -= Time.deltaTime;

        }
        public void ChangeText(string text)
        {
            _label.text = text;
            _timeElapsed = timeOnScreen;
        }
    }
}
