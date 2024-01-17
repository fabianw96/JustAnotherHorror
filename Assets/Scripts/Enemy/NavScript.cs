using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace Enemy
{
    public class NavScript : MonoBehaviour
    {
        [SerializeField] private Transform player;
        [SerializeField] private LayerMask playerLayer;
        [SerializeField] private List<Transform> waypoints;
        [SerializeField] private float detectionRange = 5f;

        private NavMeshAgent agent;
        private NavMeshHit hit;
    
        // Start is called before the first frame update
        void Start()
        {
            agent = GetComponent<NavMeshAgent>();
        }

        // Update is called once per frame
        void Update()
        {
            if (!agent.Raycast(player.position, out hit))
            {
                if (hit.distance <= detectionRange)
                {
                    agent.speed = 3;
                    agent.SetDestination(player.position);
                    
                }
                else
                {
                    StopAllCoroutines();
                    StartCoroutine(StartPatrol(agent.destination));
                }
            }
            else
            {
                StopAllCoroutines();
                StartCoroutine(StartPatrol(agent.destination));
            }
        }

        IEnumerator StartPatrol(Vector3 lastDestination)
        {
            agent.SetDestination(lastDestination);
            yield return new WaitForSeconds(5);
            // Debug.Log("returning to patrol!");
        }
    
    }
}
