using UnityEngine;

public class CountDownState : IState
{
    private readonly GameManager _gameManager;
    private static float _timer;
    private bool _hasEnteredState;

    public CountDownState(GameManager gameManager)
    {
        _gameManager = gameManager;
        _hasEnteredState = false;
        gameManager.OnResetRace += ResetCountDownState;
    }

    ~CountDownState()
    {
        _gameManager.OnResetRace -= ResetCountDownState;
    }
    
    private const float TIME_TO_START_RACE = 5f;
    
    public static float CountDownTimer => _timer;

    public void OnEnter()
    {
        if (!_hasEnteredState)
        {
            _hasEnteredState = true;
            _timer = TIME_TO_START_RACE;
            _gameManager.LockPlayers(true);
            _gameManager.ResetPlayersWayPoints();
            _gameManager.GetAllPlayers();
        }
        
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
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
    
    public void ResetCountDownState()
    {
        _hasEnteredState = false;
    }
}