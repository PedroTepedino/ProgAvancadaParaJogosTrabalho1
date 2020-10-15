using DG.Tweening;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class BowlingBallGameObject : MonoBehaviour
{
    private Tween _spawnTween;
    private Rigidbody _rigidbody;

    [SerializeField] private float _acceleration;
    [SerializeField] private float _initialForce;
    private Vector3 _forceDirection;

    private int _hitsInLifetime = 0;
    [SerializeField] private int _hitsBeforeDespawn = 3;

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
        
        _rigidbody.velocity = Vector3.zero;
        
        _spawnTween.Play();
        _rigidbody.AddForce(transform.forward * _initialForce, ForceMode.Impulse);
        _hitsInLifetime = 0;
    }
    
    private void OnCollisionEnter(Collision other)
    {
        
        
        if (other.gameObject.CompareTag("Runner"))
        {
            other.gameObject.GetComponent<AbstractRunner>().ApplyForce(this._forceDirection);
            other.gameObject.GetComponent<AbstractRunner>().Stun();
            _spawnTween.SmoothRewind();
        }
        else if (other.gameObject.CompareTag("Wall"))
        {
            _hitsInLifetime++;
            Debug.Log(_hitsInLifetime);
            if (_hitsInLifetime >= _hitsBeforeDespawn)
            {
                _rigidbody.velocity = Vector3.zero;
                _spawnTween.SmoothRewind();
            }
        }
    }
}
