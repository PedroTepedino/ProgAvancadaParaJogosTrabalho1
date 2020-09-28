using System.Collections;
using UnityEngine;

public class CountDownPanel : AbstractMenuPanel
{
    protected override void HandleGameStateChanged(IState state)
    {
        if (state is CountDownState)
        {
            StartCoroutine(WaitForFadeOut());
        }
    }

    private IEnumerator WaitForFadeOut()
    {
        yield return new WaitUntil(() => FadeInOutSceneTransition.Instance.FadeOutCompleted);
        _panel.SetActive(true);
    }

    public void ListenAnimationEnded()
    {
        _panel.SetActive(false);
    }
}