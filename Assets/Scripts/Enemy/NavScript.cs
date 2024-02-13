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
        [SerializeField] private GameObject player;
        private LayerMask _playerLayer;
        [SerializeField] private List<Transform> waypoints;
        private Transform _currentDest;
        
        [Header("Range and Time")]
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
            Debug.Log(_playerLayer.value);
            _distanceToPlayer = Vector3.Distance(player.transform.position, transform.position);
            EnemyAnimationState();
            
            if (_distanceToPlayer <= detectionRange && player.layer == _playerLayer)
            {
                //punktprodukt mathf.dot
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
                if (agent.remainingDistance <= 1f) 
                {
                    _patrolling = false;
                    agent.speed = 0;
                    StopCoroutine(StayIdle());
                    StartCoroutine(StayIdle());
                }
            }
        }

        private void EnemyAnimationState()
        {
            animator.SetFloat(_speedHash, agent.velocity.magnitude);
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
            yield return new WaitForSeconds(5);
            _chasing = false;
            _patrolling = true;
        }
    }
}
