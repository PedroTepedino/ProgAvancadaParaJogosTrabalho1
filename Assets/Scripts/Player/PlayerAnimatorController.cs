using UnityEngine;
using Random = UnityEngine.Random;

public class PlayerAnimatorController : MonoBehaviour
{
    [SerializeField] private Animator _animator;
    [SerializeField] private Rigidbody _rigidbody;
    [SerializeField] private AbstractRunner _player;

    private bool _alternateIdleIsRunning = false;
    
    private float TimeToIdle => Random.Range(4f, 6f);
    private float _timer;

    private static readonly int ForwardSpeed = Animator.StringToHash("ForwardSpeed");
    private static readonly int RotationInput = Animator.StringToHash("RotationInput");
    private static readonly int PlayAltarnateIdle = Animator.StringToHash("PlayAltarnateIdle");

    private readonly int _alternateIdleState = Animator.StringToHash("IdleVariations");
    private static readonly int Win = Animator.StringToHash("Win");
    private static readonly int Lose = Animator.StringToHash("Lose");
    private static readonly int Stunned = Animator.StringToHash("Stunned");

    private void OnEnable()
    {
        _timer = TimeToIdle;
        ChooseRandomIdleAnimation.OnIdleAnimation += ListenOnIdleAnimation;
    }
    
    private void OnDisable()
    {
        ChooseRandomIdleAnimation.OnIdleAnimation -= ListenOnIdleAnimation;
    }

    private void ListenOnIdleAnimation(Animator animator, bool alternateIdleRunning)
    {
        if (animator == _animator)
        {
            _alternateIdleIsRunning = alternateIdleRunning;
        }
    }

    private void Update()
    {
        SetSpeeds();
        SetAlternateIdle();
        SetStunned();
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

    private void SetSpeeds()
    {
        var velocity = _rigidbody.velocity;
        velocity.y = 0;
        
        _animator.SetFloat(ForwardSpeed, _rigidbody.LocalForwardVelocity());
        _animator.SetFloat(RotationInput, PlayerInput.Instance.Horizontal);
    }

    private void SetStunned()
    {
        _animator.SetBool(Stunned, _player.Stunned);
    }

    public void WinLose(bool win)
    {
        _animator.SetBool(win ? Win : Lose, true);
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

        if (_player == null)
        {
            _player = this.GetComponent<AbstractRunner>();
        }
    }
}