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
        [SerializeField] private Transform finalStandWp;

        [Header("Range and Time")] 
        private const float DefaultStoppingDistance = 0f;
        private const float HiddenStoppingDistance = 2f;
        private bool _isTimeTicking = true;
        [SerializeField] private float tameTimer = 300f;
        [SerializeField] private float timeToNextChase = 120f;
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
        private bool _isPatrolling;
        private bool _isChasing;
        private bool _isReturningToLair;
        private float _idleTime;
        private float _chaseTime;
        private Vector3 _dest;
        private float _distanceToPlayer;
        private bool _isPlayerHidden;
        

        private readonly int _speedHash = Animator.StringToHash("Speed");

        private void Awake()
        {
            _playerLayer = LayerMask.NameToLayer("Player");
            _isPatrolling = true;
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

            if (_isTimeTicking)
            {
                tameTimer -= Time.deltaTime;
            }

            if (tameTimer <= 0f)
            {
                _isTimeTicking = false;
                MoveToPlayer();
            }
            
            if (_distanceToPlayer <= detectionRange && player.layer == _playerLayer && Vector3.Dot(forward, toPlayer) > 0)
            {
                if (!agent.Raycast(player.transform.position, out _hit))
                {
                    MoveToPlayer();
                }
            }

            if (_isChasing)
            {
                _isTimeTicking = false;
                _dest = player.transform.position;
                agent.destination = _dest;
                agent.speed = chaseSpeed;
                float distance = Vector3.Distance(_dest, agent.transform.position);

                if (player.layer != _playerLayer && !_isPlayerHidden && agent.stoppingDistance <= HiddenStoppingDistance)
                {
                    StartCoroutine(StandStill());
                }
                
                if (distance <= catchDistance && player.layer == _playerLayer)
                {
                    _isChasing = false;
                    _isPatrolling = true;
                    GameplayManager.Instance.GameOver(true);
                }
            }

            if (_isPatrolling)
            {
                _dest = _currentDest.position;
                agent.destination = _dest;
                agent.speed = patrolSpeed;
                if (agent.remainingDistance <= 1f && _isPatrolling) 
                {
                    _isPatrolling = false;
                    agent.speed = 0;
                    StopCoroutine(StayIdle());
                    StartCoroutine(StayIdle());
                }
            }

            if (_isReturningToLair)
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
            _isChasing = false;
            _isPatrolling = false;
            _isReturningToLair = true;
            agent.destination = lairWaypoint.position;
        }

        public void StartFinalStand()
        {
            StopAllCoroutines();
            _isChasing = false;
            _isPatrolling = false;
            agent.Warp(finalStandWp.position);
            agent.destination = _playerPosition;
            agent.speed = chaseSpeed;
        }

        private void MoveToPlayer()
        {
            StopAllCoroutines();
            _isPlayerHidden = false;
            _isPatrolling = false;
            _isChasing = true;
            tameTimer = timeToNextChase;
        }

        private IEnumerator StayIdle()
        {
            _idleTime = Random.Range(minIdleTime, maxIdleTime);
            yield return new WaitForSeconds(_idleTime);
            _isPatrolling = true;
            _currentDest = waypoints[Random.Range(0, waypoints.Count)];
        }

        private IEnumerator StandStill()
        {
            _isPlayerHidden = true;
            Debug.Log("Pre-Wait");
            _isReturningToLair = false;
            yield return new WaitForSeconds(5);
            Debug.Log("Post-Wait");
            agent.stoppingDistance = DefaultStoppingDistance;
            _isChasing = false;
            _isPatrolling = true;
            _isTimeTicking = true;
        }
    }
}