using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;

public class AiAnimationAnimatorController : MonoBehaviour
{
    [SerializeField] private Animator _animator;
    [SerializeField] private NavMeshAgent _agent;
    [SerializeField] private Rigidbody _rigidbody;
    [SerializeField] private AbstractRunner _runner;

    private float TimeToIdle => Random.Range(4f, 6f);
    private float _timer;
    
    private static readonly int ForwardSpeed = Animator.StringToHash("ForwardSpeed");
    private static readonly int RotationInput = Animator.StringToHash("RotationInput");
    private static readonly int PlayAltarnateIdle = Animator.StringToHash("PlayAltarnateIdle");

    private float _lastFrameRotation;
    private float _rotationSpeedEstimate = 0f;
    
    private bool _alternateIdleIsRunning = false;
    private static readonly int Win = Animator.StringToHash("Win");
    private static readonly int Lose = Animator.StringToHash("Lose");
    private static readonly int Stunned = Animator.StringToHash("Stunned");

    private void OnEnable()
    {
        _timer = TimeToIdle;
        _lastFrameRotation = this.transform.rotation.eulerAngles.y;
        ChooseRandomIdleAnimation.OnIdleAnimation += ListenOnIdleAnimation;
    }

    private void OnDisable()
    {
        ChooseRandomIdleAnimation.OnIdleAnimation -= ListenOnIdleAnimation;
    }

    private void Update()
    {
        CalculateRotationSpeed();
        SetSpeeds();
        SetAlternateIdle();
        SetStunned();
    }

    private void ListenOnIdleAnimation(Animator animator, bool alternateIdleRunning)
    {
        if (animator == _animator)
        {
            _alternateIdleIsRunning = alternateIdleRunning;
        }
    }

    private void SetSpeeds()
    {
        var velocity = _agent.velocity;
        velocity.y = 0;
        
        _animator.SetFloat(ForwardSpeed,  _agent.enabled?_agent.LocalForwardVelocity():_rigidbody.LocalForwardVelocity());
        _animator.SetFloat(RotationInput, _rotationSpeedEstimate);

    }

    private void CalculateRotationSpeed()
    {
        var thisFrameRotation = this.transform.rotation.eulerAngles.y;
        _rotationSpeedEstimate = thisFrameRotation - _lastFrameRotation;
        _lastFrameRotation = thisFrameRotation;
    }

    private void SetAlternateIdle()
    {
        if (_alternateIdleIsRunning) return;

        if (_timer < 0f)
        {
            _animator.SetTrigger(PlayAltarnateIdle);
            _timer = TimeToIdle;
        }
        
        if (_rigidbody.GroundVelocity().magnitude < 0.1f)
        {
            _timer -= Time.deltaTime;
        }
        else
        {
            _timer = TimeToIdle;
        }
    }

    public void SetWin()
    {
        _animator.SetBool(Win, true);
    }

    public void SetLose()
    {
        _animator.SetBool(Lose, true);
    }

    private void SetStunned()
    {
        _animator.SetBool(Stunned, _runner.Stunned);
    }

    private void OnValidate()
    {
        if (_agent == null)
        {
            _agent = this.GetComponent<NavMeshAgent>();
        }

        if (_rigidbody == null)
        {
            _rigidbody = this.GetComponent<Rigidbody>();
        }

        if (_rigidbody == null)
        {
            _runner = this.GetComponent<AbstractRunner>();
        }
    }
}