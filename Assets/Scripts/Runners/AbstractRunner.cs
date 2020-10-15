using System;
using UnityEngine;
using UnityEngine.AI;

public abstract class AbstractRunner : MonoBehaviour
{
    [SerializeField] protected float _runningSpeed;
    [SerializeField] protected float _rotationSpeed;
    [SerializeField] protected float _walkingSpeed;
    [SerializeField] protected float _acceleration = 2;
    [SerializeField] protected float _maxRunSpeed = 10;
    [SerializeField] protected float _maxWalkSpeed = 3;
    [SerializeField] protected float _stunTime = 3f;
    [SerializeField] protected ParticleSystem _stunParticles;

    private int _currentLapsCount = 0;
    public int CurrentLapsCount => _currentLapsCount;
    public static event Action<AbstractRunner, int> OnLap;
    
    protected Transform _currentWayPoint;
    protected int _currentWayPointIndex = 0;

    private float _lastFloorSpeed = -1;
    
    protected bool _stunned = false;

    public bool Stunned => _stunned;

    protected RunnerPowerUpManager _powerUpManager;
    public float DistanceToWayPoint => (_currentWayPoint.position - this.transform.position).magnitude;

    public float PositionIndex => (3 - _currentLapsCount) +
                                  (Waypoints.Instance.WaypointsCount - _currentWayPointIndex) + 
                                  DistanceToWayPoint;
        

    protected virtual void OnEnable()
    {
        GameManager.Instance.OnHandleRunnerLockState += Lock;
        GameManager.Instance.OnResetRunnerStatus += ListenOnResetRunnerStatus;
    }

    protected virtual void OnDisable()
    {
        GameManager.Instance.OnHandleRunnerLockState -= Lock;
        GameManager.Instance.OnResetRunnerStatus -= ListenOnResetRunnerStatus;
    }

    private void ListenOnResetRunnerStatus()
    {
        _currentLapsCount = 0;
        _currentWayPointIndex = Waypoints.Instance.GetFirstWayPoint(out _currentWayPoint);
    }

    public float GetCurrentFloorVelocity()
    {
        if (NavMesh.SamplePosition(this.transform.position, out NavMeshHit hit, 10f, NavMesh.AllAreas))
        {
            if (GameManager.Instance.FloorTypes.ContainsKey(hit.mask))
            {
                _lastFloorSpeed = GameManager.Instance.FloorTypes[hit.mask].Velocity;
                return _lastFloorSpeed;
            }
        }

        if (_lastFloorSpeed > 0) return _lastFloorSpeed;

        // In Case Everything Fails
        return 6;
    }

    public abstract void Stun();
    public abstract void ApplyForce(Vector3 force);

    protected abstract void Lock(bool lockState);

    public virtual void ReachedWayPoint(Transform waypoint)
    {
        if (waypoint != _currentWayPoint) return;
        
        if (waypoint == Waypoints.Instance.GetFinishLine())
            Lap();

        _currentWayPointIndex = Waypoints.Instance.GetNextWayPoint(out _currentWayPoint, _currentWayPointIndex);
        
    }

    protected void Lap()
    {
        _currentLapsCount++;
        OnLap?.Invoke(this, CurrentLapsCount);
    }

    public abstract void FinishRace(int position);
}