using System.Diagnostics;
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
    [SerializeField] private LayerMask _layermasks;

    private float _lastFloorSpeed = -1;
    
    protected bool _stunned = false;

    protected RunnerPowerUpManager _powerUpManager;

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
}