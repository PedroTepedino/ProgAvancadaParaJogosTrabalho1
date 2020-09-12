using System;
using System.ComponentModel;
using System.Data.Common;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Runner : MonoBehaviour
{
    [SerializeField] private float _runningSpeed;
    [SerializeField] private float _rotationSpeed;
    [SerializeField] private float _walkingSpeed;
    [SerializeField] private float _acceleration = 2;
    [SerializeField] private float _maxRunSpeed = 10;
    [SerializeField] private float _maxWalkSpeed = 3;

    private Rigidbody _rigidbody;

    private Mover _runner;
    private Mover _walker;
    private Mover _mover;

    public float Acceleration => _acceleration;
    public float RotationSpeed => _rotationSpeed;

    private void Awake()
    {
        _rigidbody = this.GetComponent<Rigidbody>();
        _runner = new Mover(this, _maxRunSpeed);
        _walker = new Mover(this, _walkingSpeed);

        _mover = _runner;
    }

    private void OnEnable()
    {
        PlayerInput.Instance.OnWalking += ListenOnWalk;
    }

    private void OnDisable()
    {
        PlayerInput.Instance.OnWalking -= ListenOnWalk;   
    }

    private void Update()
    {
        PlayerInput.Instance.Tick();
    }

    private void FixedUpdate()
    {
        _mover.Tick();
    }

    private void ListenOnWalk(bool walking)
    {
        _mover = walking ? _walker : _runner;
    }
}

public interface IMover
{
    void Tick();
}



public class Mover : IMover
{
    private readonly Rigidbody _rigidbody;
    private readonly Transform _transform;
    
    private readonly float _acceleration;
    private readonly float _rotationSpeed;
    private readonly float _maxRunForwardVelocity;
    private readonly float _maxRunBackWardsVelocity;

    private Vector3 GroundVelocity => new Vector3(_rigidbody.velocity.x, 0, _rigidbody.velocity.z);

    public bool MovingForward => Vector3.Dot(GroundVelocity, _transform.forward) > 0;

    public Mover(Runner runner, float maxSpeed)
    {
        _rigidbody = runner.gameObject.GetComponent<Rigidbody>();
        _transform = runner.transform;
        _rotationSpeed = runner.RotationSpeed;
        _maxRunForwardVelocity = maxSpeed;
        _maxRunBackWardsVelocity = maxSpeed / 2f;
        _acceleration = runner.Acceleration;
    }

    public void Tick()
    {
        // float speed = 0;
        // if (PlayerInput.Instance.Vertical > 0.1f)
        // {
        //     speed = !PlayerInput.Instance.Walking ? _runningSpeed : _walkingSpeed;
        // }
        // else if (PlayerInput.Instance.Vertical < -0.1f)
        // {
        //     speed = -(!PlayerInput.Instance.Walking ? _runningSpeed : _walkingSpeed) / 2f;
        // }
        //
        // Vector3 velocity = _transform.forward * speed;
        //
        // velocity.y = _rigidbody.velocity.y;
        //
        // _rigidbody.velocity = velocity;

        var groundVelocity = GroundVelocity;
        var currentMaxVelocity = MovingForward ? _maxRunForwardVelocity : _maxRunBackWardsVelocity;

        if (groundVelocity.magnitude < currentMaxVelocity)
        {
            _rigidbody.AddForce(_transform.forward * _acceleration * PlayerInput.Instance.Acceleration);
        }

        if (groundVelocity.magnitude > 0.1f)
        {
            _transform.Rotate(_transform.up, _rotationSpeed * Time.fixedDeltaTime * PlayerInput.Instance.Horizontal);
        }
    }
}

public static class RigidBodyExtensions
{
    public static bool MovingForward(this Rigidbody rigidbody)
    {
        // If not moving Calculates as if going forward
        if (rigidbody.velocity.magnitude < 0.01 && rigidbody.velocity.magnitude > -0.01) return true;
        
        var groundVelocity = rigidbody.velocity;
        groundVelocity.y = 0f;
        return Vector3.Dot(groundVelocity, rigidbody.transform.forward) > 0;
    }
}