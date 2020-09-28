using System.Diagnostics;
using DG.Tweening;
using UnityEngine;
using Debug = UnityEngine.Debug;

[RequireComponent(typeof(Collider))]
public class BowlingBallGameObject : MonoBehaviour
{
    private Tween _spawnTween;
    private Rigidbody _rigidbody;

    [SerializeField] private float _acceleration;
    [SerializeField] private float _initialForce;
    private Vector3 _forceDirection;

    private void Awake()
    {
        _spawnTween = this.transform.DOScale(3f, 0.2f)
            .From(0f)
            .SetEase(Ease.OutBack)
            .SetAutoKill(false)
            .SetRelative(false);
        _spawnTween.OnRewind(() => this.gameObject.SetActive(false));
        _spawnTween.Rewind();
        _rigidbody = this.GetComponent<Rigidbody>();
    }

    private void FixedUpdate()
    {
        _forceDirection = _rigidbody.velocity.normalized;
        _forceDirection.y = 0;
        _rigidbody.AddForce(_forceDirection * _acceleration);
    }
    
    private void OnEnable()
    {
        if (_spawnTween.IsBackwards()) _spawnTween.isBackwards = false;
        
        _spawnTween.Play();
        _rigidbody.AddForce(transform.forward * _initialForce, ForceMode.Impulse);
    }
    
    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("Runner"))
        {
            other.gameObject.GetComponent<AbstractRunner>().ApplyForce(this._forceDirection);
            other.gameObject.GetComponent<AbstractRunner>().Stun();
            _spawnTween.SmoothRewind();
        }
    }
}
