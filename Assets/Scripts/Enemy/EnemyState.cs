using UnityEngine;

namespace Enemy
{
    public enum EState
    {
        Default = 0,
        Idle,
        Patrolling,
        Chasing,
        Lair
    }
    
    public class EnemyState : MonoBehaviour
    {
        private EState _currenState = EState.Default;

        public void ChangeState(EState state)
        {
            _currenState = state;
        }
        
        public EState GetCurrentState()
        {
            return _currenState;
        }
    }
}
