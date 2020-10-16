using DG.Tweening;
using TMPro;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class KoGlove : MonoBehaviour
{
    private Tween _tween;
    [SerializeField] private Rigidbody _rigidbody;
    [SerializeField] private float _velocity = 3f;

    private void Awake()
    {
        _tween = this.transform.DOScale(10f, 0.3f).From(0f).SetAutoKill(false).SetEase(Ease.OutBack);
        _tween.onRewind += ()=>this.gameObject.SetActive(false);
        _tween.Rewind();
    }

    private void OnEnable()
    {
        _rigidbody.velocity = this.transform.forward * _velocity;
        _tween.isBackwards = false;
        _tween.Play();
    }

    private void Retract()
    {
        _rigidbody.velocity = Vector3.zero;
        _tween.SmoothRewind();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Runner"))
        {
            var runner = other.GetComponent<AbstractRunner>();
            runner.Stun();
        }
        Retract();
    }

    private void OnValidate()
    {
        if (_rigidbody == null)
        {
            _rigidbody = this.GetComponent<Rigidbody>();
        }
    }
}