using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Player : AbstractRunner
{
    private bool _finishedRace = false;
    private Rigidbody _rigidbody;
    private PlayerAnimatorController _playerAnimator;

    private Mover _runner;
    private Mover _walker;
    private IMover _mover;

    public float Acceleration => _acceleration;
    public float RotationSpeed => _rotationSpeed;
    public float MAXRunSpeed => _maxRunSpeed;

    protected void Awake()
    {
        _rigidbody = this.GetComponent<Rigidbody>();
        _powerUpManager = this.GetComponent<RunnerPowerUpManager>();
        _playerAnimator = this.GetComponent<PlayerAnimatorController>();
        _runner = new Mover(this, _maxRunSpeed);
        _walker = new Mover(this, _walkingSpeed);

        _mover = _runner;
    }

    protected override void OnEnable()
    {
        base.OnEnable();
        _stunned = false;
        _finishedRace = false;
        PlayerInput.Instance.OnWalking += ListenOnWalk;
    }

    private void OnDisable()
    {
        base.OnDisable();
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

        _mover?.Tick();
    }

    private void ListenOnWalk(bool walking)
    {
        if (_finishedRace) return;
        
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
        this._rigidbody.AddForce(force, ForceMode.Impulse);
    }

    protected override void Lock(bool lockState)
    {
        if (lockState)
        {
            this._rigidbody.constraints = RigidbodyConstraints.FreezeAll;
        }
        else
        {
            // Freezes the RotationX = 16, the RotationZ = 66 and PositionY = 4
            // code: RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ | RigidbodyConstraints.FreezePositionY
            this._rigidbody.constraints = (RigidbodyConstraints) 84;
        }
    }

    public override void FinishRace(int position)
    {
        _finishedRace = true;
        _mover = null;
        this.ApplyForce(this.transform.forward * 5f);
        _playerAnimator.WinLose(position <= 0);
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


