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

    public bool MovingForward => Vector3.Dot(_rigidbody.GroundVelocity(), _transform.forward) > 0;

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

public static class RigidBodyExtensions
{
    public static bool MovingForward(this Rigidbody rigidbody)
    {
        // If not moving Calculates as if going forward
        if (rigidbody.velocity.magnitude < 0.1f && rigidbody.velocity.magnitude > -0.1f) return true;
        
        var groundVelocity = rigidbody.velocity;
        groundVelocity.y = 0f;
        return Vector3.Dot(groundVelocity, rigidbody.transform.forward) > 0;
    }

    public static float LocalForwardVelocity(this Rigidbody rigidbody)
    {
        float magnitude = Vector3.Project(rigidbody.GroundVelocity(), rigidbody.transform.forward).magnitude;
        if (magnitude < 0.1f) return 0f;

        return magnitude * (Vector3.Dot(rigidbody.GroundVelocity(), rigidbody.transform.forward) >= 0 ? 1f : -1f);
    }

    public static Vector3 GroundVelocity(this Rigidbody rigidbody)
    {
        Vector3 velocity = rigidbody.velocity;
        return new Vector3(velocity.x, 0, velocity.z);
    }

    public static float LocalSideVelocity(this Rigidbody rigidbody)
    {
        float magnitude = Vector3.Project(rigidbody.GroundVelocity(), rigidbody.transform.right).magnitude;
        if (magnitude < 0.1f) return 0f;

        return magnitude * (Vector3.Dot(rigidbody.GroundVelocity(), rigidbody.transform.right) >= 0 ? 1f : -1f);
    }
}