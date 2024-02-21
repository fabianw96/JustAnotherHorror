using ScriptableObjects.Events;
using UnityEngine;

namespace Interactables
{
    public class FinalDoor : MonoBehaviour
    {
        [SerializeField] private ScriptableEvent triggeredEvent;

        public void OnFinalStand()
        {
            triggeredEvent.RaiseEvent();
        }
    }
}
