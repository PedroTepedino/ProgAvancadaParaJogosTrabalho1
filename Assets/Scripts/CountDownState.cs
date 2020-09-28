using UnityEngine;

public class CountDownState : IState
{
    private readonly GameManager _gameManager;
    private static float _timer;

    public CountDownState(GameManager gameManager)
    {
        _gameManager = gameManager;
    }

    private const float TIME_TO_START_RACE = 5f;
    
    public static float CountDownTimer => _timer;

    public void OnEnter()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        _timer = TIME_TO_START_RACE;
        Time.timeScale = 1f;
    }

    public void Tick()
    {
        if (!FadeInOutSceneTransition.Instance.FadeOutCompleted) return;
        
        _timer -= Time.deltaTime;
        if (_timer <= 0)
        {
            _gameManager.CountDownFinished();
        }
    }

    public void OnExit()
    {
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }
}