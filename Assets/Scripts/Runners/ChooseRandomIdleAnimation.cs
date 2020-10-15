using System;
using UnityEngine;
using Random = UnityEngine.Random;

public class ChooseRandomIdleAnimation : StateMachineBehaviour
{
    public bool AlternateIdleRunning { get; private set; } = false;

    public static event Action<Animator, bool> OnIdleAnimation;

    private int _lastAnimation = -1;
    private static readonly int AlternateIdle = Animator.StringToHash("AlternateIdle");

    // OnStateMachineEnter is called when entering a state machine via its Entry Node
    public override void OnStateMachineEnter(Animator animator, int stateMachinePathHash)
    {
        var animationIndex = Random.Range(0, 6);

        if (animationIndex == _lastAnimation)
        {
            animationIndex++;
            if (animationIndex >= 5)
                animationIndex = 0;
        }
        
        animator.SetInteger(AlternateIdle, animationIndex);

        _lastAnimation = animationIndex;
        OnIdleAnimation?.Invoke(animator, true);
    }
    
    // OnStateMachineExit is called when exiting a state machine via its Exit Node
    public override void OnStateMachineExit(Animator animator, int stateMachinePathHash)
    {
        var transform = animator.transform;
        transform.localPosition = Vector3.zero;
        transform.localRotation = Quaternion.identity;
        OnIdleAnimation?.Invoke(animator, false);
    }
    
    // OnStateEnter is called before OnStateEnter is called on any state inside this state machine
    //override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    
    //}

    // OnStateUpdate is called before OnStateUpdate is called on any state inside this state machine
    //override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    
    //}

    // OnStateExit is called before OnStateExit is called on any state inside this state machine
    //override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    
    //}

    // OnStateMove is called before OnStateMove is called on any state inside this state machine
    //override public void OnStateMove(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    
    //}

    // OnStateIK is called before OnStateIK is called on any state inside this state machine
    //override public void OnStateIK(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    
    //}

    
}
