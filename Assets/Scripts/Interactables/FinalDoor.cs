using ScriptableObjects.Events;
using UnityEngine;

namespace Interactables
{
    public class FinalDoor : MonoBehaviour
    {
        [SerializeField] private ScriptableEvent triggeredEvent;
        private bool _isFinalTriggered;

        public void OnFinalStand()
        {
            if (_isFinalTriggered)
                return;
            triggeredEvent.RaiseEvent();
            _isFinalTriggered = true;
        }
    }
}
