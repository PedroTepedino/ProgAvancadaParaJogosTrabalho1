using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(Animator))]
public class Ai : AbstractRunner
{
    [SerializeField] private Animator _animator;
    [SerializeField] private NavMeshAgent _agent;
    private Rigidbody _rigidbody;

    public float StunTime => _stunTime;
    public ParticleSystem StunParticles => _stunParticles;
    public Transform CurrentWayPoint => _currentWayPoint;



    private static readonly int CanMove = Animator.StringToHash("CanMove");
    private static readonly int Stunned = Animator.StringToHash("Stunned");
    private static readonly int WinState = Animator.StringToHash("Win");
    private static readonly int LoseState = Animator.StringToHash("Lose");
    private float _wayPointDeviation ;
    private float _canReactiveAgentTimer;
    [SerializeField] private float _timeUntilCanReactivateAgent = 1f;

    private void Awake()
    {
        _rigidbody = this.GetComponent<Rigidbody>();
        _wayPointDeviation =  Random.Range(-5f, 5f);
    }

    private void Start()
    {
        _currentWayPointIndex = Waypoints.Instance.GetFirstWayPoint(out _currentWayPoint);
    }

    private void Update()
    {
        if (_rigidbody.isKinematic) return;

        if (_canReactiveAgentTimer < 0)
        {
            if (_rigidbody.velocity.magnitude <= 0.7f * this.GetCurrentFloorVelocity())
            {
                _rigidbody.isKinematic = true;
                _agent.enabled = true;
            }
        }
        else
        {
            _canReactiveAgentTimer -= Time.deltaTime;
        }
    }

    public override void Stun()
    {
        _animator.SetBool(Stunned, true);
    }

    public override void ApplyForce(Vector3 force)
    {
        _agent.enabled = false;
        _rigidbody.isKinematic = false;
        _rigidbody.AddForce(force, ForceMode.Impulse);
        _canReactiveAgentTimer = _timeUntilCanReactivateAgent;
    }

    public override void ReachedWayPoint(Transform waypoint)
    {
        if (waypoint != _currentWayPoint) return;
        
        if (waypoint == Waypoints.Instance.GetFinishLine())
            Lap();
        
        _currentWayPointIndex = Waypoints.Instance.GetNextWayPoint(out _currentWayPoint, _currentWayPointIndex);

        _wayPointDeviation = Random.Range(-5f, 5f);
        
        UpdateDestination();
    }

    public override void FinishRace(int position)
    {
        if (position > 0)
        {
            this.Lose();
        }
        else
        {
            this.Win();
        }
    }

    protected override void Lock(bool lockState)
    {
        _animator.SetBool(CanMove, !lockState);
    }

    public void UpdateDestination(Vector3 position)
    {
        if (!_agent.enabled) return;
        
        if (NavMesh.SamplePosition(position, out NavMeshHit navMeshHit, 100f, -1))
        {
            _agent.SetDestination(navMeshHit.position);
        }
    }

    public void UpdateDestination()
    {
        if (!_agent.enabled) return;
        
        if (NavMesh.SamplePosition(_currentWayPoint.position + (_currentWayPoint.right * _wayPointDeviation), out NavMeshHit navMeshHit, 100f, -1))
        {
            _agent.SetDestination(navMeshHit.position);
        }
    }
    
    public void StopAi()
    {
        _agent.speed = 0;
    }
    
    public void ReleaseAi()
    {
        UpdateAiSpeed();
    }

    public void UpdateAiSpeed()
    {
        _agent.speed = this.GetCurrentFloorVelocity();
    }

    public void Win()
    {
        _animator.SetTrigger(WinState);
    }

    public void Lose()
    {
        _animator.SetTrigger(LoseState);
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

    public void SetStunned(bool stunned)
    {
        _stunned = stunned;
    }
}