using UnityEngine;

public class ExitState : IState
{ 
    public void OnEnter()
    {
        Application.Quit();
    }

    public void Tick()
    {
    }

    public void OnExit()
    {
    }
}