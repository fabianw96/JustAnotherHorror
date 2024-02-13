using System.Collections.Generic;
using UnityEngine;

namespace ScriptableObjects.Events
{
    [CreateAssetMenu(menuName = "ScriptableObjects/ScriptableEvent")]
    public class ScriptableEvent : ScriptableObject, ISerializationCallbackReceiver
    {
        private List<ScriptableEventListener> listenerList = new();

        public void Register(ScriptableEventListener listener)
        {
            listenerList.Add(listener);
        }

        public void UnRegister(ScriptableEventListener listener)
        {
            if (listenerList.Count <= 0) return;

            listenerList.Remove(listener);
        }

        public void RaiseEvent()
        {
            foreach (var listener in listenerList)
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
            listenerList.Clear();
        }
    }
}
