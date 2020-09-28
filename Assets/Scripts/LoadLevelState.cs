using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadLevelState : IState
{
    public static string LevelToLoad;
    public bool FinishLoading() => _operations.TrueForAll(operation => operation.isDone && operation.allowSceneActivation);

    private List<AsyncOperation> _operations = new List<AsyncOperation>();

    public void OnEnter()
    {
        FadeInOutSceneTransition.Instance.FadeIn();
        _operations.Add(SceneManager.LoadSceneAsync(LevelToLoad));
        _operations.Add(SceneManager.LoadSceneAsync("Ui", LoadSceneMode.Additive));
        _operations.ForEach(operation => operation.allowSceneActivation = false);
    }

    public void Tick()
    {
        if (FadeInOutSceneTransition.Instance.FadeInCompleted)
        {
            _operations.ForEach(operation => operation.allowSceneActivation = true);
        }
    }

    public void OnExit()
    {
        LevelToLoad = null;
        FadeInOutSceneTransition.Instance.FadeOut();
    }
}