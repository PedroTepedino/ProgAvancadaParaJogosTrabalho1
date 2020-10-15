using UnityEngine;

public class FinishRace : IState
{
    private static float _timer = 0;

    public static float Timer => _timer;

    public void OnEnter()
    {
        _timer = 10f;
    }

    public void Tick()
    {
        _timer -= Time.deltaTime;
        if (_timer <= 0f)
        {
            RestartRace();
        }
    }

    public void OnExit()
    {
    }

    private void RestartRace()
    {
        LoadLevelState.LevelToLoad = "GamePlay";
    }
}