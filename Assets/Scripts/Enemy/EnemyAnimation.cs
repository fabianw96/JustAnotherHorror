using UnityEngine;

namespace Enemy
{
    public class EnemyAnimation : MonoBehaviour
    {
        private readonly int _speedHash = Animator.StringToHash("Speed");
        [SerializeField] private Animator animator;

        public void UpdateAnimationState(float speed)
        {
            animator.SetFloat(_speedHash, speed);
        }
    }
}
