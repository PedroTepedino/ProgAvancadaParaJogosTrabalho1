using UnityEngine;

public class PauseState : IState
{ 
    public void OnEnter()
    {
        Time.timeScale = 0f;
    }

    public void Tick()
    {
    }

    public void OnExit()
    {
        
        Time.timeScale = 1f;
    }
}