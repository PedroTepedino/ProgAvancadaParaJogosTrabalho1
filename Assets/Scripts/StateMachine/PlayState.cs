using UnityEngine;

public class PlayState : IState
{
    private readonly GameManager _gameManager;

    public PlayState(GameManager gameManager)
    {
        _gameManager = gameManager;
    }

    public void OnEnter()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        _gameManager.LockPlayers(false);
    }

    public void Tick()
    {
    }

    public void OnExit()
    {
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }
}