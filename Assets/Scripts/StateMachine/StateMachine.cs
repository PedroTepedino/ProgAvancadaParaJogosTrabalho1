using System;
using System.Collections.Generic;
using UnityEngine;

public class StateMachine
{
    public event Action<IState> OnStateChanged;
    private Dictionary<IState, List<StateTransition>> _stateTransitions = new Dictionary<IState, List<StateTransition>>();
    private IState _currentState;
    public IState LastState { get; private set; } = null;
    public IState CurrentState => _currentState;

    public void AddTransition(IState from, IState to, Func<bool> condition)
    {
        if (_stateTransitions.ContainsKey(from) == false)
            _stateTransitions[from] = new List<StateTransition>();
        
        var stateTransition = new StateTransition(from, to, condition);
        _stateTransitions[from].Add(stateTransition);
    }

    public void SetState(IState state)
    {
        if (_currentState == state)
            return;
        
        _currentState?.OnExit();

        LastState = _currentState;
        _currentState = state;
        //Debug.Log($"Change to {state}");
        _currentState.OnEnter();
        
        OnStateChanged?.Invoke(_currentState);
    }

    public void Tick()
    {
        StateTransition transition = CheckForTransition();

        if (transition != null)
        {
            SetState(transition.To);
        }
        
        _currentState.Tick();
    }

    private StateTransition CheckForTransition()
    {
        if (_stateTransitions.ContainsKey(_currentState))
        {
            foreach (var transition in _stateTransitions[_currentState])
            {
                if (transition.From == _currentState && transition.Condition())
                    return transition;
            }
        }

        return null;
    }
}