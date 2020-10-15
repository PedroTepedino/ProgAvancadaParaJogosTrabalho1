using System;

public class StateTransition
{
    public readonly IState From;
    public readonly IState To;
    public readonly Func<bool> Condition;
    
    public StateTransition(IState from, IState to, Func<bool> condition )
    {
        // Apparently from is a contextual C# keyword;
        From = @from;
        To = to;
        Condition = condition;
    }
}