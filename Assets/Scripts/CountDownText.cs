using DG.Tweening;
using TMPro;
using UnityEngine;

public class CountDownText : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _text;
    private CountDownPanel _countDownPanel;

    private Tween _tween;
    private Tween _fadeTween;

    private int _currentTimer = 5;

    private void Awake()
    {
        _countDownPanel = this.GetComponentInParent<CountDownPanel>();
        _tween = this.transform.DOScale(1.2f, 0.5f).From(0.7f).SetAutoKill(false).SetLoops(2, LoopType.Yoyo).SetEase(Ease.OutCubic);
        _tween.Rewind();

        _fadeTween = _text.DOFade(0, 0.5f).From(1).SetAutoKill(false).SetEase(Ease.OutExpo).SetDelay(1f);
        _fadeTween.onComplete += _countDownPanel.ListenAnimationEnded;
        _fadeTween.Rewind();
    }

    private void OnEnable()
    {
        _currentTimer = 6;
    }

    void Update()
    {
        var timer = Mathf.CeilToInt(CountDownState.CountDownTimer);
        
        if (timer != _currentTimer)
        {
            _tween.Restart();
        }
        
        _currentTimer = timer;
        string aux = _currentTimer.ToString();
        if (_currentTimer == 0)
        {
            aux = "start";
            if (!_fadeTween.IsPlaying())
            {
                _fadeTween.Play();
            }
        }

        _text.text = aux;
    }

    private void OnValidate()
    {
        if (_text == null)
        {
            _text = this.GetComponent<TextMeshProUGUI>();
        }
    }
}
