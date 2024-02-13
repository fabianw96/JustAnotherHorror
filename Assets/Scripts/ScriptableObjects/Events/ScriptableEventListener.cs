using UnityEngine;
using UnityEngine.Events;

namespace ScriptableObjects.Events
{
    public class ScriptableEventListener : MonoBehaviour
    {
        [SerializeField] private ScriptableEvent scriptableEvent;
        [SerializeField] private UnityEvent eventResponse;

        private void Awake()
        {
            scriptableEvent.Register(this);
        }

        private void OnDestroy()
        {
            scriptableEvent.UnRegister(this);
        }

        public void OnEventRaised()
        {
            eventResponse.Invoke();
        }
    }
}
