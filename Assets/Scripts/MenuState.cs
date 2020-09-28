using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuState : IState
{

    public void OnEnter()
    {
        Time.timeScale = 1f;
        LoadLevelState.LevelToLoad = null;
        SceneManager.LoadScene("Menu");
    }

    public void Tick()
    {
    }

    public void OnExit()
    {
    }
}