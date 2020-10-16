using DG.Tweening;
using UnityEngine;

public class HiddenWall : MonoBehaviour
{
    private Tween _hideAnimation;
    
    private void Awake()
    {
        _hideAnimation = this.transform.DOMove(this.transform.position, 0.5f)
            .From(this.transform.position + new Vector3(0f, -3f, 0f))
            .SetEase(Ease.OutBack)
            .SetAutoKill(false)
            .SetRelative(true);
        _hideAnimation.Rewind();
    }
    
    public void SetHiddenState(bool isHidden)
    {
        if (isHidden)
        {
            _hideAnimation.SmoothRewind();
        }
        else
        {
            _hideAnimation.isBackwards = false;
            _hideAnimation.Play();
        }
    }
}