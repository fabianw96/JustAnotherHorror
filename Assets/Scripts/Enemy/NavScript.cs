using System.Collections;
using UnityEngine;
using UnityEngine.AI;

namespace Enemy
{
    public class NavScript : MonoBehaviour
    {
        [SerializeField] private Transform player;
        [SerializeField] private LayerMask playerLayer;

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
                agent.SetDestination(player.position);
            }
            else
            {
                StartCoroutine(StartPatrol());
                StopAllCoroutines();
            }
        }

        IEnumerator StartPatrol()
        {
            yield return new WaitForSeconds(5);
            Debug.Log("returning to patrol!");
        }
    
    }
}
