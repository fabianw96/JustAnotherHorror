using System.Collections;
using System.Collections.Generic;
using Managers;
using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;

namespace Enemy
{
    public class EnemyAi : MonoBehaviour
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
        [SerializeField] private EnemyAnimation enemyAnimation;
        [SerializeField] private EnemyState enemyState;
        [SerializeField] private NavMeshAgent agent;

        [Header("Animations")] 
        private AnimationEvent _animationEvent;
        
        private NavMeshHit _hit;
        private int _rndNum;
        private float _idleTime;
        private Vector3 _dest;
        private float _distanceToPlayer;
        private bool _isPlayerHidden;

        private void Awake()
        {
            //determine layer from layer mask
            _playerLayer = LayerMask.NameToLayer("Player");
            enemyState.ChangeState(EState.Patrolling);
            _currentDest = waypoints[Random.Range(0, waypoints.Count)];
        }

        // Update is called once per frame
        private void Update()
        {
            //track current player position and distance to the player
            _playerPosition = player.transform.position;
            _distanceToPlayer = Vector3.Distance(_playerPosition, transform.position);
            Vector3 forward = transform.TransformDirection(Vector3.forward);
            Vector3 toPlayer = _playerPosition - transform.position;
            
            //set the current animationstate
            enemyAnimation.UpdateAnimationState(agent.velocity.magnitude);
            
            CheckTameTimer();
            FindPlayerInFront(forward, toPlayer);
            EnemyChase();
            EnemyPatrol();

            ReturnToLairCheck();
        }
        private void ReturnToLairCheck()
        {
            //enemy runs back to his lair
            if (enemyState.GetCurrentState() != EState.Lair)
                return;
            
            if (!(agent.remainingDistance <= 1f)) return;
            StartCoroutine(StandStill());
        }
        private void CheckTameTimer()
        {
            //time counting down to a forced chase
            if (_isTimeTicking)
            {
                tameTimer -= Time.deltaTime;
            }

            if (!(tameTimer <= 0f))
                return;
            
            _isTimeTicking = false;
            MoveToPlayer();
        }
        private void FindPlayerInFront(Vector3 forward, Vector3 toPlayer)
        {
            // enemy can't see beyond a certain distance, player only "visible" when on correct layer and in front of enemy
            if (!(_distanceToPlayer <= detectionRange) || player.layer != _playerLayer || !(Vector3.Dot(forward, toPlayer) > 0))
                return;
            
            //agent can't raycast through objects
            if (!agent.Raycast(player.transform.position, out _hit))
            {
                MoveToPlayer();
            }
        }
        private void EnemyChase()
        {
            if (enemyState.GetCurrentState() != EState.Chasing)
                return;
            
            _isTimeTicking = false;
            _dest = player.transform.position;
            agent.destination = _dest;
            agent.speed = chaseSpeed;
            float distance = Vector3.Distance(_dest, agent.transform.position);

            //enemy walk to last known player position and waits
            if (player.layer != _playerLayer && !_isPlayerHidden && agent.stoppingDistance <= HiddenStoppingDistance)
            {
                StartCoroutine(StandStill());
            }

            //if player is caught game over
            if (!(distance <= catchDistance) || player.layer != _playerLayer)
                return;
            
            enemyState.ChangeState(EState.Patrolling);
            //player fade to black, sounds, restart & save progress to last key
            GameplayManager.Instance.GameOver(true);
        }
        private void EnemyPatrol()
        {
            if (enemyState.GetCurrentState() != EState.Patrolling)
                return;
            
            // agent generates random location and walks there, idles and then repeat
            _dest = _currentDest.position;
            agent.destination = _dest;
            agent.speed = patrolSpeed;
            if (agent.remainingDistance <= 1f && enemyState.GetCurrentState() == EState.Patrolling)
            {
                enemyState.ChangeState(EState.Idle);
                agent.speed = 0;
                StopCoroutine(StayIdle());
                StartCoroutine(StayIdle());
            }
        }

        public void MoveToLair()
        {
            //called when player enters lair
            StopAllCoroutines();
            _isTimeTicking = false;
            enemyState.ChangeState(EState.Lair);
            agent.destination = lairWaypoint.position;
        }

        public void StartFinalStand()
        {
            //called when player interacts with door after final key is picked up
            StopAllCoroutines();
            _isTimeTicking = false;
            agent.Warp(finalStandWp.position);
            agent.destination = _playerPosition;
            agent.speed = chaseSpeed;
            enemyState.ChangeState(EState.Chasing);
        }

        private void MoveToPlayer()
        {
            StopAllCoroutines();
            _isPlayerHidden = false;
            enemyState.ChangeState(EState.Chasing);
            tameTimer = timeToNextChase;
        }

        private IEnumerator StayIdle()
        {
            _idleTime = Random.Range(minIdleTime, maxIdleTime);
            yield return new WaitForSeconds(_idleTime);
            enemyState.ChangeState(EState.Patrolling);
            _currentDest = waypoints[Random.Range(0, waypoints.Count)];
        }

        private IEnumerator StandStill()
        {
            _isPlayerHidden = true;
            yield return new WaitForSeconds(5);
            agent.stoppingDistance = DefaultStoppingDistance;
            enemyState.ChangeState(EState.Patrolling);
            _isTimeTicking = true;
        }
    }
}
