using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuState : IState
{

    private AsyncOperation _loadOperation;

    public void OnEnter()
    {
        Time.timeScale = 1f;
        LoadLevelState.LevelToLoad = null;
        var activeScene = SceneManager.GetActiveScene();
        if (activeScene.buildIndex != 0)
        {
            FadeInOutSceneTransition.Instance.FadeIn();
            _loadOperation = SceneManager.LoadSceneAsync("Menu");
            _loadOperation.allowSceneActivation = false;
        }
    }

    public void Tick()
    {
        if (_loadOperation != null && FadeInOutSceneTransition.Instance.FadeInCompleted)
        {
            _loadOperation.allowSceneActivation = true;
            FadeInOutSceneTransition.Instance.FadeOut();
            _loadOperation = null;
        }
    }

    public void OnExit()
    {
    }
}