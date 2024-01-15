using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Serialization;

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
            StartCoroutine(StartPartrol());
            StopAllCoroutines();
        }
    }

    IEnumerator StartPartrol()
    {
        yield return new WaitForSeconds(5);
        Debug.Log("returning to patrol!");
    }
    
}
