using System.Collections;
using System.Collections.Generic;
using Managers;
using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;

namespace Enemy
{
    public class NavScript : MonoBehaviour
    {
        [Header("Transforms")]
        private Transform _currentDest;
        private LayerMask _playerLayer;
        [SerializeField] private GameObject player;
        private Vector3 _playerPosition;
        [SerializeField] private List<Transform> waypoints;
        [SerializeField] private Transform lairWaypoint;

        [Header("Range and Time")] 
        private const float DefaultStoppingDistance = 0f;
        private const float HiddenStoppingDistance = 2f;
        [SerializeField] private float detectionRange = 5f;
        [SerializeField] private float catchDistance = 1f;
        [SerializeField][Range(1f, 15f)] private float minIdleTime;
        [SerializeField][Range(1f, 15f)] private float maxIdleTime;

        [Header("Speed")] 
        [SerializeField] private float chaseSpeed;
        [SerializeField] private float patrolSpeed;

        [Header("Components")]
        [SerializeField] private Animator animator;
        [SerializeField] private NavMeshAgent agent;

        [Header("Animations")] 
        private AnimationEvent _animationEvent;
        
        private NavMeshHit _hit;
        private int _rndNum;
        private bool _patrolling;
        private bool _chasing;
        private bool _returningToLair;
        private float _idleTime;
        private float _chaseTime;
        private Vector3 _dest;
        private float _distanceToPlayer;
        private bool _isPlayerHidden;
        

        private readonly int _speedHash = Animator.StringToHash("Speed");

        private void Awake()
        {
            _playerLayer = LayerMask.NameToLayer("Player");
            _patrolling = true;
            _currentDest = waypoints[Random.Range(0, waypoints.Count)];
        }

        // Update is called once per frame
        void Update()
        {
            _playerPosition = player.transform.position;
            _distanceToPlayer = Vector3.Distance(_playerPosition, transform.position);
            Vector3 forward = transform.TransformDirection(Vector3.forward);
            Vector3 toPlayer = _playerPosition - transform.position;
            EnemyAnimationState();
            
            if (_distanceToPlayer <= detectionRange && player.layer == _playerLayer && Vector3.Dot(forward, toPlayer) > 0)
            {
                if (!agent.Raycast(player.transform.position, out _hit))
                {
                    _isPlayerHidden = false;
                    _patrolling = false;
                    StopCoroutine(StayIdle());
                    _chasing = true;
                }
            }

            if (_chasing)
            {
                _dest = player.transform.position;
                agent.destination = _dest;
                agent.speed = chaseSpeed;
                float distance = Vector3.Distance(_dest, agent.transform.position);
                
                if (player.layer != _playerLayer && !_isPlayerHidden)
                {
                    StopAllCoroutines();
                    agent.stoppingDistance = HiddenStoppingDistance;
                    StartCoroutine(StandStill());
                }
                
                if (distance <= catchDistance && player.layer == _playerLayer)
                {
                    _chasing = false;
                    _patrolling = true;
                    GameplayManager.Instance.GameOver(true);
                }
            }

            if (_patrolling)
            {
                _dest = _currentDest.position;
                agent.destination = _dest;
                agent.speed = patrolSpeed;
                if (agent.remainingDistance <= 1f && _patrolling) 
                {
                    _patrolling = false;
                    agent.speed = 0;
                    StopCoroutine(StayIdle());
                    StartCoroutine(StayIdle());
                }
            }

            if (_returningToLair)
            {
                if (!(agent.remainingDistance <= 1f)) return;
                
                Debug.Log("CLOSE");
                StartCoroutine(StandStill());
            }
        }

        private void EnemyAnimationState()
        {
            animator.SetFloat(_speedHash, agent.velocity.magnitude);
        }

        public void MoveToLair()
        {
            StopAllCoroutines();
            _chasing = false;
            _patrolling = false;
            _returningToLair = true;
            agent.destination = lairWaypoint.position;
        }

        private IEnumerator StayIdle()
        {
            _idleTime = Random.Range(minIdleTime, maxIdleTime);
            yield return new WaitForSeconds(_idleTime);
            _patrolling = true;
            _currentDest = waypoints[Random.Range(0, waypoints.Count)];
        }

        private IEnumerator StandStill()
        {
            _isPlayerHidden = true;
            Debug.Log("Pre-Wait");
            _returningToLair = false;
            yield return new WaitForSeconds(5);
            Debug.Log("Post-Wait");
            agent.stoppingDistance = DefaultStoppingDistance;
            _chasing = false;
            _patrolling = true;
        }
    }
}
