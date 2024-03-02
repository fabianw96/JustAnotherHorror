using UnityEngine;

namespace Enemy
{
    public class EnemyAnimation : MonoBehaviour
    {
        private readonly int _speedHash = Animator.StringToHash("Speed");
        [SerializeField] private Animator animator;

        /// <summary>
        /// Method used to set the float of the animator to play the correct animation
        /// </summary>
        public void UpdateAnimationState(float speed)
        {
            animator.SetFloat(_speedHash, speed);
        }
    }
}
