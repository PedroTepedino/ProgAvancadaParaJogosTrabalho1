using DG.Tweening;
using TMPro;
using UnityEngine;

public class RestartCountDown : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _text;
    private int _currentTimer = 10;

    private Tween _tween;
    
    private void Awake()
    {
        _tween = this.transform.DOScale(1.2f, 0.5f).From(0.7f).SetAutoKill(false).SetLoops(2, LoopType.Yoyo).SetEase(Ease.OutCubic);
        _tween.Rewind();
    }
    
    void Update()
    {
        var timer = Mathf.CeilToInt(FinishRace.Timer);
        
        if (timer != _currentTimer)
        {
            _tween.Restart();
        }
        
        _currentTimer = timer;

        _text.text = _currentTimer.ToString();;
    }
    
    private void OnEnable()
    {
        _currentTimer = 11;
    }

    
    private void OnValidate()
    {
        if (_text == null)
        {
            _text = this.GetComponent<TextMeshProUGUI>();
        }
    }
}