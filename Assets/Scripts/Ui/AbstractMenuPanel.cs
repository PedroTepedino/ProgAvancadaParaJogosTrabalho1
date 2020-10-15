using UnityEngine;

public abstract class AbstractMenuPanel : MonoBehaviour
{
    [SerializeField] protected GameObject _panel;

    protected virtual void Awake()
    {
        _panel.SetActive(false);
        GameManager.Instance.OnGameStateChanged += HandleGameStateChanged;
    }

    private void OnDestroy()
    {
        GameManager.Instance.OnGameStateChanged -= HandleGameStateChanged;
    }

    protected abstract void HandleGameStateChanged(IState state);

    protected virtual void OnValidate()
    {
        if (_panel == null && this.transform.childCount > 0)
        {
            _panel = this.transform.GetChild(0).gameObject;
        }
    }
}