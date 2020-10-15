using System;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class PlayerUi : MonoBehaviour
{
    [SerializeField] private Image _image;

    //TODO: Serialize a dictionary, with Odin if Possible
    [SerializeField] private Sprite _bowlingBallSprite;
    [SerializeField] private Sprite _swiftPotionSprite;
    [SerializeField] private Sprite _stunBoxSprite;

    [SerializeField] private Sprite _defaultSprite;
    
    //TODO: Se if I can use Odin in the project to serialize this
    private Dictionary<Type, Sprite> _spriteDictionary;
    
    private Tween _tween;

    private void Awake()
    {
        _tween = this.transform.DOScale(1f, 0.5f).SetAutoKill(false).From(0f).SetEase(Ease.OutExpo);
        _tween.Rewind();
        
        _spriteDictionary = new Dictionary<Type, Sprite>()
        {
            {typeof(BowlingBall), _bowlingBallSprite},
            {typeof(SwiftnessPotion), _swiftPotionSprite},
            {typeof(StunBox), _stunBoxSprite}
        };
    }

    private void OnEnable()
    {
        PlayerInstanceReference.Instance.PowerUpManager.OnPowerUpChanged += OpenCloseUi;
    }

    private void OnDisable()
    {
        if (PlayerInstanceReference.Instance != null)
            PlayerInstanceReference.Instance.PowerUpManager.OnPowerUpChanged -= OpenCloseUi;
    }

    private void OpenCloseUi(PowerUp powerUp)
    {
        if (powerUp == null)
        {
            _tween.SmoothRewind();
        }
        else
        {
            _image.sprite = SelectSpriteImage(powerUp.GetType());
            _tween.Restart();
        }
    }

    private Sprite SelectSpriteImage(Type powerUpType)
    {
        if (!_spriteDictionary.ContainsKey(powerUpType))
            return _defaultSprite;
        
        return _spriteDictionary[powerUpType];
    }
}
