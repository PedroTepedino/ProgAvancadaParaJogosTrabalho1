using UnityEngine;

public class PlayState : IState
{ 
    public void OnEnter()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
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