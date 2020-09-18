using System;
using System.Collections;
using System.Runtime.CompilerServices;
using DG.Tweening;
using TMPro;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class PickUpBoxes : MonoBehaviour
{
    private Tween _tween;
    private Collider _collider;

    [SerializeField] private Transform _modelTransform;
    [SerializeField] private float _tweenTime = 0.5f;

    [SerializeField] private float _spawnTime = 10f;

    private WaitForSeconds _waitSeconds;

    private void Awake()
    {
        _collider = this.GetComponent<Collider>();
        _waitSeconds = new WaitForSeconds(_spawnTime);
    }

    private void Start()
    {
        _tween = _modelTransform.DOScale(1f, _tweenTime)
            .SetAutoKill(false)
            .From(0f)
            .SetEase(Ease.OutQuad)
            .OnComplete(() => _collider.enabled = true)
            .OnRewind(() => StartCoroutine(SpawnTimer()));
    }

    private IEnumerator SpawnTimer()
    {
        yield return _waitSeconds;

        _tween.timeScale = 1f;
        _tween.Restart();
    }

    private void OnTriggerEnter(Collider other)
    {
        _collider.enabled = false;
        _tween.timeScale = 3f;
        _tween.SmoothRewind();
    }

    private void OnValidate()
    {
        _modelTransform = this.transform.GetChild(0);
    }
}
