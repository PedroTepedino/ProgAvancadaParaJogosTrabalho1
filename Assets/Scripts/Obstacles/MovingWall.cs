using System;
using UnityEngine;
using DG.Tweening;

public class MovingWall : MonoBehaviour
{
    [SerializeField] private Transform _wall;
    [SerializeField] private Transform _endPoint;

    [Header("Tween Options")]
    [SerializeField] private float _animationTime = 10f;
    [SerializeField] private bool _speedBased = true;
    [SerializeField] private Ease _easeType;
    
    private Tween _animation;

    private void Awake()
    {
        _animation = _wall.DOMove(_endPoint.position, _animationTime).SetSpeedBased(_speedBased).SetAutoKill(false)
            .SetEase(_easeType).SetLoops(-1, LoopType.Yoyo).From(this.transform.position);
        _animation.Rewind();
    }

    private void OnEnable()
    {
        _animation.Play();
    }

    private void OnDisable()
    {
        _animation.Pause();
    }
}