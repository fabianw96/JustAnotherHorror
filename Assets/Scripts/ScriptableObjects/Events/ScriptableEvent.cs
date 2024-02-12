using System.Collections.Generic;
using UnityEngine;

namespace ScriptableObjects.Events
{
    [CreateAssetMenu(menuName = "ScriptableObjects/ScriptableEvent")]
    public class ScriptableEvent : ScriptableObject
    {
        private List<ScriptableEventListener> listenerList;

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
    }
}
