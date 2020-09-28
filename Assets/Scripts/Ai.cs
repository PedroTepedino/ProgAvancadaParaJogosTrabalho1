using System;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UIElements;

[RequireComponent(typeof(Animator))]
public class Ai : MonoBehaviour
{
    private Transform _currentWayPoint;
    private int _currentWayPointIndex = 0;

    [SerializeField] private Animator _animator;
    [SerializeField] private NavMeshAgent _agent;

    private void Start()
    {
        _currentWayPointIndex = Waypoints.Instance.GetFirstWayPoint(out _currentWayPoint);
        UpdateDestination();
    }
    
    public void ReachedWayPoint(Transform waypointReached)
    {
        if (_currentWayPoint == waypointReached)
        {
            _currentWayPointIndex = Waypoints.Instance.GetNextWayPoint(out _currentWayPoint, _currentWayPointIndex);
            UpdateDestination();
        }
    }

    public void UpdateDestination()
    {
        NavMeshHit navMeshHit;
        if (NavMesh.SamplePosition(_currentWayPoint.position, out navMeshHit, 100f, -1))
        {
            _agent.SetDestination(navMeshHit.position);
        }
    }
    
    private void OnValidate()
    {
        if (_animator == null)
        {
            _animator = this.GetComponent<Animator>();
        }

        if (_agent == null)
        {
            _agent = this.GetComponent<NavMeshAgent>();
        }
    }
}