using System.Runtime.Remoting.Channels;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class FinishPanel : AbstractMenuPanel
{
    [SerializeField] private Image _image;
    [SerializeField] private TextMeshProUGUI _winLoseText;
    [SerializeField] private Sprite _winSprite;
    [SerializeField] private Sprite _loseSprite;

    protected override void HandleGameStateChanged(IState state)
    {
        if (state is FinishRace)
        {
            SetWinSprite();
            _panel.SetActive(true);
        }
        else
        {
            _panel.SetActive(false);
        }
    }
    
    private void SetWinSprite()
    {
        _image.sprite = GameManager.Instance.HasPlayerWon ? _winSprite : _loseSprite;
        _winLoseText.text = GameManager.Instance.HasPlayerWon ? "You Won!" : "You Lost!";
    }
    
    protected override void OnValidate()
    {
        base.OnValidate();

        if (_image == null)
        {
            _image = _panel.GetComponent<Image>();
        }

        if (_winLoseText == null)
        {
            _winLoseText = _panel.GetComponentInChildren<TextMeshProUGUI>();
        }
    }
}