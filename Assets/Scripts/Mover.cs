using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mover : MonoBehaviour
{
    [SerializeField] private List<Transform> waypoints;
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private LayerMask playerLayer;
    private int _currentWaypoint;

    // Start is called before the first frame update
    void Start()
    {
        if (waypoints.Count <= 0) return;
        _currentWaypoint = 0;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        HandleMovement();
    }

    private void OnCollisionEnter(Collision other)
    {
        other.transform.SetParent(transform);
    }

    private void OnTriggerExit(Collider other)
    {
        other.transform.SetParent(null);
    }


    private void HandleMovement()
    {
        transform.position = Vector3.MoveTowards(transform.position, waypoints[_currentWaypoint].transform.position,
            (moveSpeed * Time.deltaTime));

        if (Vector3.Distance(waypoints[_currentWaypoint].transform.position, transform.position) <= 0)
        {
            _currentWaypoint++;
        }

        if (_currentWaypoint != waypoints.Count) return;
        waypoints.Reverse();
        _currentWaypoint = 0;
    }
}
