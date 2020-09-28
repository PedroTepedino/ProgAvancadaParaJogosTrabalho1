using System.Collections;
using UnityEngine;
using UnityEngine.AI;
using Debug = UnityEngine.Debug;

[RequireComponent(typeof(Rigidbody))]
public class Player : AbstractRunner
{
    private Rigidbody _rigidbody;

    private Mover _runner;
    private Mover _walker;
    private IMover _mover;

    public float Acceleration => _acceleration;
    public float RotationSpeed => _rotationSpeed;
    public float MAXRunSpeed => _maxRunSpeed;

    private void Awake()
    {
        _rigidbody = this.GetComponent<Rigidbody>();
        _powerUpManager = this.GetComponent<RunnerPowerUpManager>();
        _runner = new Mover(this, _maxRunSpeed);
        _walker = new Mover(this, _walkingSpeed);

        _mover = _runner;
    }

    private void OnEnable()
    {
        _stunned = false; 
        PlayerInput.Instance.OnWalking += ListenOnWalk;
    }

    private void OnDisable()
    {
        PlayerInput.Instance.OnWalking -= ListenOnWalk;   
    }

    private void Update()
    {
        PlayerInput.Instance.Tick();
        PowerUpInput();
    }

    private void FixedUpdate()
    {
        if (_stunned) return;

        _mover.Tick();
    }

    private void ListenOnWalk(bool walking)
    {
        _mover = walking ? _walker : _runner;
    }

    public override void Stun()
    {
        _stunned = true;
        _stunParticles.Play();

        StartCoroutine(StopSunParticles());
    }

    public override void ApplyForce(Vector3 force)
    {
        this._rigidbody.AddForce(force);
    }

    private IEnumerator StopSunParticles()
    {
         yield return new WaitForSeconds(_stunTime);

         _stunParticles.Stop();
         _stunned = false;
    }

    private void PowerUpInput()
    {
        if (PlayerInput.Instance.PowerUp)
        {
            _powerUpManager.UsePowerUp();
        }
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

    public bool MovingForward => Vector3.Dot(_rigidbody.GroundVelocity(), _transform.forward) > 0;

    public Mover(Player player, float maxSpeed)
    {
        _rigidbody = player.gameObject.GetComponent<Rigidbody>();
        _transform = player.transform;
        _rotationSpeed = player.RotationSpeed;
        _maxRunForwardVelocity = maxSpeed;
        _maxRunBackWardsVelocity = maxSpeed / 2f;
        _acceleration = player.Acceleration;
    }

    public void Tick()
    {
        var groundVelocityMag = _rigidbody.GroundVelocity().magnitude;
        var currentMaxVelocity = MovingForward ? _maxRunForwardVelocity : _maxRunBackWardsVelocity;

        if (groundVelocityMag < currentMaxVelocity)
        {
            var currentAcceleration = _acceleration * (1f - (groundVelocityMag / currentMaxVelocity));
            _rigidbody.AddForce(_transform.forward * currentAcceleration * PlayerInput.Instance.Acceleration);
        }

        if (groundVelocityMag > 0.1f)
        {
            _transform.Rotate(_transform.up, _rotationSpeed * Time.fixedDeltaTime * PlayerInput.Instance.Horizontal);
        }
    }
}


