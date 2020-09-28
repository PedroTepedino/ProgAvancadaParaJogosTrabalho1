using DG.Tweening;
using UnityEngine;

public class UiScaleTween : MonoBehaviour
{
    private Tween _fadeIn;

    private void Awake()
    {
        _fadeIn = this.transform.DOScale(1f, 0.5f).From(0f).SetAutoKill(false).SetEase(Ease.OutExpo).SetUpdate(UpdateType.Normal, true);
        _fadeIn.Rewind();
    }

    private void OnEnable()
    {
        _fadeIn.Restart();   
    }
}