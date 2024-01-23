using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

namespace Enemy
{
    public class NavScript : MonoBehaviour
    {
        [Header("Transforms")]
        [SerializeField] private Transform player;
        [SerializeField] private List<Transform> waypoints;
        private Transform currentDest;
        
        [Header("Range and Time")]
        [SerializeField] private float detectionRange = 5f;
        [SerializeField] private float catchDistance = 1f;
        [SerializeField][Range(1f, 10f)] private float minIdleTime;
        [SerializeField][Range(1f, 10f)] private float maxIdleTime;
        [SerializeField][Range(1f, 10f)] private float minChaseTime;
        [SerializeField][Range(1f, 10f)] private float maxChaseTime;
        

        [Header("Speed")] 
        [SerializeField] private float chaseSpeed;
        [SerializeField] private float patrolSpeed;

        [Header("Components")]
        [SerializeField] private Animator animator;
        [SerializeField] private NavMeshAgent agent;
        
        private NavMeshHit hit;
        private int rndNum;
        private bool patrolling;
        private bool chasing;
        private float idleTime;
        private float chaseTime;
        private Vector3 dest;
        private float agentDistance;
        

        private readonly int speedHash = Animator.StringToHash("Speed");

        private void Awake()
        {
            patrolling = true;
            rndNum = Random.Range(0, waypoints.Count);
            currentDest = waypoints[rndNum];
            Debug.Log(rndNum);
        }

        // Update is called once per frame
        void Update()
        {
            EnemyAnimationState();
            if (agent.Raycast(player.position, out hit)) return;
            if (hit.distance <= detectionRange)
            {
                patrolling = false;
                StopCoroutine(nameof(StayIdle));
                StopCoroutine(nameof(ChaseRoutine));
                StartCoroutine(nameof(ChaseRoutine));
                chasing = true;
            }

            if (chasing == true)
            {
                dest = player.position;
                agent.destination = dest;
                agent.speed = chaseSpeed;
                float distance = Vector3.Distance(dest, agent.transform.position);
                if (distance <= catchDistance)
                {
                    Debug.Log("You dead");
                    // chasing = false;
                }
            }

            if (patrolling == true)
            {
                
                dest = currentDest.position;
                agent.destination = dest;
                agent.speed = patrolSpeed;
                agentDistance = agent.remainingDistance;
                // Debug.Log($"{agent.remainingDistance} ---- {agent.stoppingDistance}");
                if (agent.remainingDistance <= agent.stoppingDistance)
                {
                    patrolling = false;
                    agent.speed = 0;
                    // StopCoroutine(nameof(StayIdle));
                    StartCoroutine(nameof(StayIdle));
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
