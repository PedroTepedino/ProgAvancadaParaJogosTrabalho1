using DG.Tweening;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class StunBoxGameObject : MonoBehaviour, IPickable
{
    [SerializeField] private ParticleSystem _pickUpParticleSystem;
    private Collider _collider;
    private Tween _spawnTween;
    private Sequence _pickUpTween;
    [SerializeField] private AnimationCurve _animationCurve;

    private void Awake()
    {
        _collider = this.GetComponent<Collider>();
        
        _spawnTween = this.transform.DOScale(1f, 0.5f)
            .SetEase(Ease.OutBack)
            .SetAutoKill(false)
            .From(0f)
            .OnComplete(() => _collider.enabled = true);
        _spawnTween.Rewind();

        _pickUpTween = DOTween.Sequence();
        _pickUpTween.OnStart(() => _collider.enabled = false);
        _pickUpTween.Append(this.transform.DOScale(0f, 0.2f).SetEase(_animationCurve).From(1f)
            .SetAutoKill(false));
        _pickUpTween.OnComplete(() => this.gameObject.SetActive(false));
        _pickUpTween.SetAutoKill(false);
        _pickUpTween.Rewind();
    }

    private void OnEnable()
    {
        _spawnTween.Restart();
    }

    public void PickUp()
    {
        _pickUpTween.Restart();
    }

    private void OnTriggerEnter(Collider other)
    {
        PickUp();
        
        var runner = other.GetComponent<Runner>();
        if (runner)
        {
            _pickUpParticleSystem.DORestart();
            runner.Stun();
        }
    }
}