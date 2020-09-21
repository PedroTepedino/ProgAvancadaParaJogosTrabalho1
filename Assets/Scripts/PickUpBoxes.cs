using System;
using System.Collections;
using System.Reflection;
using DG.Tweening;
using UnityEngine;
using Random = UnityEngine.Random;

[RequireComponent(typeof(Collider))]
public class PickUpBoxes : MonoBehaviour, IPickable
{
    private Tween _tween;
    private Collider _collider;

    [SerializeField] private Transform _modelTransform;
    [SerializeField] private float _tweenTime = 0.5f;

    [SerializeField] private float _spawnTime = 10f;

    private WaitForSeconds _waitSeconds;

    private Type[] _powerUpTypes = new[] {typeof(BowlingBall), typeof(SwiftnessPotion), typeof(StunBox)};

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
            .SetEase(Ease.OutBack)
            .OnComplete(() => _collider.enabled = true)
            .OnRewind(() => StartCoroutine(SpawnTimer()));
    }

    public void PickUp()
    {
        _collider.enabled = false;
        _tween.timeScale = 3f;
        _tween.SmoothRewind();
    }

    private IEnumerator SpawnTimer()
    {
        yield return _waitSeconds;

        _tween.timeScale = 1f;
        _tween.Restart();
    }

    public PowerUp PickUpPower(RunnerPowerUpManager picker)
    {
        return SudoFactory(picker, _powerUpTypes[Random.Range(0, _powerUpTypes.Length)]);
    }

    private PowerUp SudoFactory(RunnerPowerUpManager picker, Type powerUpType)
    {
        if (powerUpType == typeof(PowerUp)) return null;
        
        Type[] constructorParameters = {typeof(RunnerPowerUpManager)};
        ConstructorInfo constructorInfo = powerUpType.GetConstructor(constructorParameters);

        if (constructorInfo != null)
            return constructorInfo.Invoke(new object[] {picker}) as PowerUp;
        
        return null;
    }

    private void OnValidate()
    {
        _modelTransform = this.transform.GetChild(0);
    }
}