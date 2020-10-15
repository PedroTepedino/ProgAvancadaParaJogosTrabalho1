using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lose : StateMachineBehaviour
{
    private AiAnimationAnimatorController _animationController;
    private Ai _ai;
    
    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (_animationController == null)
        {
            _animationController = animator.GetComponent<AiAnimationAnimatorController>();
        }
        
        if (_ai == null)
        {
            _ai = animator.GetComponent<Ai>();
        }
        
        _animationController.SetLose();
        _ai.UpdateDestination(_ai.transform.position + _ai.transform.forward * 5f);
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    //override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    
    //}

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    //override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    
    //}

    // OnStateMove is called right after Animator.OnAnimatorMove()
    //override public void OnStateMove(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    // Implement code that processes and affects root motion
    //}

    // OnStateIK is called right after Animator.OnAnimatorIK()
    //override public void OnStateIK(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    // Implement code that sets up animation IK (inverse kinematics)
    //}
}
