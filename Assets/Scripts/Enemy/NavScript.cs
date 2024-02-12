using System;
using System.Collections;
using System.Collections.Generic;
using Managers;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

namespace Enemy
{
    public class NavScript : MonoBehaviour
    {
        [Header("Transforms")]
        [SerializeField] private GameObject player;
        private LayerMask _playerLayer;
        [SerializeField] private List<Transform> waypoints;
        private Transform currentDest;
        
        [Header("Range and Time")]
        [SerializeField] private float detectionRange = 5f;
        [SerializeField] private float catchDistance = 1f;
        [SerializeField][Range(1f, 15f)] private float minIdleTime;
        [SerializeField][Range(1f, 15f)] private float maxIdleTime;
        [SerializeField][Range(1f, 5f)] private float minChaseTime;
        [SerializeField][Range(1f, 5f)] private float maxChaseTime;
        

        [Header("Speed")] 
        [SerializeField] private float chaseSpeed;
        [SerializeField] private float patrolSpeed;

        [Header("Components")]
        [SerializeField] private Animator animator;
        [SerializeField] private NavMeshAgent agent;

        [Header("Animations")] 
        private AnimationEvent _animationEvent;
        
        private NavMeshHit hit;
        private int rndNum;
        private bool patrolling;
        private bool chasing;
        private float idleTime;
        private float chaseTime;
        private Vector3 dest;
        private float distanceToPlayer;
        

        private readonly int speedHash = Animator.StringToHash("Speed");

        private void Awake()
        {
            _playerLayer = LayerMask.NameToLayer("Player");
            patrolling = true;
            rndNum = Random.Range(0, waypoints.Count);
            currentDest = waypoints[rndNum];
            Debug.Log(rndNum);  
        }

        // Update is called once per frame
        void Update()
        {
            distanceToPlayer = Vector3.Distance(player.transform.position, transform.position);
            EnemyAnimationState();
            
            if (distanceToPlayer <= detectionRange && player.layer == _playerLayer)
            {
                //punktprodukt mathf.dot
                if (!agent.Raycast(player.transform.position, out hit))
                {
                    patrolling = false;
                    StopCoroutine(StayIdle());
                    StopCoroutine(ChaseRoutine());
                    StartCoroutine(ChaseRoutine());
                    chasing = true;
                }
            }

            if (chasing)
            {
                dest = player.transform.position;
                agent.destination = dest;
                agent.speed = chaseSpeed;
                float distance = Vector3.Distance(dest, agent.transform.position);
                if (distance <= catchDistance && player.layer == _playerLayer)
                {
                    chasing = false;
                    GameManager.Instance.GameOver(true);
                }
            }

            if (patrolling)
            {
                dest = currentDest.position;
                agent.destination = dest;
                agent.speed = patrolSpeed;
                if (agent.remainingDistance <= 1f) 
                {
                    patrolling = false;
                    agent.speed = 0;
                    StopCoroutine(StayIdle());
                    StartCoroutine(StayIdle());
                }
            }
        }

        private void EnemyAnimationState()
        {
            animator.SetFloat(speedHash, agent.velocity.magnitude);
        }

        private IEnumerator StayIdle()
        {
            idleTime = Random.Range(minIdleTime, maxIdleTime);
            Debug.Log("Idling for: " + minIdleTime + " to " + maxIdleTime);
            yield return new WaitForSeconds(idleTime);
            patrolling = true;
            rndNum = Random.Range(0, waypoints.Count);
            currentDest = waypoints[rndNum];
        }

        private IEnumerator ChaseRoutine()
        {
            chaseTime = Random.Range(minChaseTime, maxChaseTime);
            yield return new WaitForSeconds(chaseTime);
            patrolling = true;
            chasing = false;
            rndNum = Random.Range(0, waypoints.Count);
            currentDest = waypoints[rndNum];
        }
    }
}
