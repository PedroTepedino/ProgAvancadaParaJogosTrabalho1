using UnityEngine;

public class RunnerAnimatorController : MonoBehaviour
{
    [SerializeField] private Animator _animator;
    [SerializeField] private Rigidbody _rigidbody;
    
    private static readonly int ForwardSpeed = Animator.StringToHash("ForwardSpeed");
    private static readonly int SideSpeed = Animator.StringToHash("SideSpeed");

    private void Update()
    {
        SetSpeeds();
    }

    private void SetSpeeds()
    {
        var velocity = _rigidbody.velocity;
        velocity.y = 0;
        
        _animator.SetFloat(ForwardSpeed, velocity.magnitude * (_rigidbody.MovingForward() ? 1 : -1));
        _animator.SetFloat(SideSpeed, PlayerInput.Instance.Horizontal);
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
