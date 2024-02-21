using System.Collections.Generic;
using UnityEngine;

namespace ScriptableObjects.Events
{
    [CreateAssetMenu(menuName = "ScriptableObjects/ScriptableEvent")]
    public class ScriptableEvent : ScriptableObject, ISerializationCallbackReceiver
    {
        private List<ScriptableEventListener> _listenerList = new();

        public void Register(ScriptableEventListener listener)
        {
            _listenerList.Add(listener);
        }

        public void UnRegister(ScriptableEventListener listener)
        {
            if (_listenerList.Count <= 0) return;

            _listenerList.Remove(listener);
        }

        public void RaiseEvent()
        {
            foreach (var listener in _listenerList)
            {
                listener.OnEventRaised();
            }
        }

        public void OnBeforeSerialize()
        {
            // throw new System.NotImplementedException();
        }

        public void OnAfterDeserialize()
        {
            _listenerList.Clear();
        }
    }
}
