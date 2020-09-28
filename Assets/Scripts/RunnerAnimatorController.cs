using UnityEngine;
using Random = UnityEngine.Random;

public class RunnerAnimatorController : MonoBehaviour
{
    [SerializeField] private Animator _animator;
    [SerializeField] private Rigidbody _rigidbody;

    private float TimeToIdle => Random.Range(4f, 6f);
    private float _timer;

    private static readonly int ForwardSpeed = Animator.StringToHash("ForwardSpeed");
    private static readonly int RotationInput = Animator.StringToHash("RotationInput");
    private static readonly int SideSpeed = Animator.StringToHash("SideSpeed");
    private static readonly int PlayAltarnateIdle = Animator.StringToHash("PlayAltarnateIdle");


    private void OnEnable()
    {
        _timer = TimeToIdle;
    }

    private void Update()
    {
        SetSpeeds();
        SetAlternateIdle();
    }

    private void SetAlternateIdle()
    {
        if (ChooseRandomIdleAnimation.AlternateIdleRunning) return;

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

    private void SetSpeeds()
    {
        var velocity = _rigidbody.velocity;
        velocity.y = 0;
        
        _animator.SetFloat(ForwardSpeed, _rigidbody.LocalForwardVelocity());
        _animator.SetFloat(RotationInput, PlayerInput.Instance.Horizontal);
        
        _animator.SetFloat(SideSpeed, _rigidbody.LocalSideVelocity());
    }

    private void OnValidate()
    {
        if (_animator == null)
        {
            _animator = this.GetComponentInChildren<Animator>();
        }

        if (_rigidbody == null)
        {
            _rigidbody = this.GetComponent<Rigidbody>();
        }
    }
}