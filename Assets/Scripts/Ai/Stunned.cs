using UnityEngine;

public class Stunned : StateMachineBehaviour
{
    private Ai _ai = null;

    private float _stunTimer = 0f;
    private static readonly int StunnedStringHash = Animator.StringToHash("Stunned");

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (_ai == null)
        {
            _ai = animator.GetComponent<Ai>();
        }

        _stunTimer = _ai.StunTime;
        
        _ai.StopAi();
        _ai.SetStunned(true);
        _ai.StunParticles.Play();
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        _stunTimer -= Time.deltaTime;

        if (_stunTimer <= 0)
        {
            animator.SetBool(StunnedStringHash, false);
        }
    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        _ai.ReleaseAi();   
        _ai.StunParticles.Stop();
        _ai.SetStunned(false);
    }

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
