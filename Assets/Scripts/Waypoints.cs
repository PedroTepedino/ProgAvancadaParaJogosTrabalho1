using System;
using UnityEngine;

public class Waypoints : MonoBehaviour
{
    public static Waypoints Instance;
    [SerializeField] private Transform[] _waypoints;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(this.gameObject);
    }

    private void OnDestroy()
    {
        if (Instance == this)
            Instance = null;
    }

    public int GetFirstWayPoint(out Transform _transform)
    {
        _transform = _waypoints[0];
        return 0;
    }

    public int GetNextWayPoint(out Transform _transform, int currentWaypoint)
    {
        currentWaypoint++;
        if (currentWaypoint >= _waypoints.Length)
        {
            currentWaypoint = 0;
        }

        _transform = _waypoints[currentWaypoint];
        return currentWaypoint;
    }
}