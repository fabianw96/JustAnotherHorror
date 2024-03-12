using PlayerScripts;
using ScriptableObjects.Events;
using UnityEngine;

namespace Enemy
{
    public class EnemyEvents : MonoBehaviour
    {
        [SerializeField] private ScriptableEvent playerCaughtEvent;

        public void CatchPlayer()
        {
            // Debug.Log("Caught player!");
            playerCaughtEvent.RaiseEvent();
        }
    }
}
