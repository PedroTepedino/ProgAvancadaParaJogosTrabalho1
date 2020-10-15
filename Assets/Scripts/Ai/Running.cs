using System.Collections;
using UnityEngine;

public class Running : StateMachineBehaviour
{
    private Ai _ai = null;
    private FieldOfView _fov = null;
    private Transform _currentPowerUpTarget = null;
    private RunnerPowerUpManager _runnerPowerUpManager = null;
    private bool _hasPowerUp;

    private Vector2 _minMaxPowerUpUsageTime = new Vector2(2f, 5f);
    

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (_ai == null)
        {
            _ai = animator.GetComponent<Ai>();
        }

        if (_fov == null)
        {
            _fov = animator.GetComponent<FieldOfView>();
        }

        if (_runnerPowerUpManager == null)
        {
            _runnerPowerUpManager = animator.GetComponent<RunnerPowerUpManager>();
        }

        _runnerPowerUpManager.OnPowerUpChanged += ListenToPowerUpChange;
        _ai.UpdateDestination();
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        _currentPowerUpTarget = _fov.FindPowerUp();

        if (_currentPowerUpTarget != null && !_hasPowerUp)
        {
            _ai.UpdateDestination(_currentPowerUpTarget.transform.position);
        }
        else
        {
            _ai.UpdateDestination();
        }

        _ai.UpdateAiSpeed();
    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        _runnerPowerUpManager.OnPowerUpChanged -= ListenToPowerUpChange;
    }

    private void ListenToPowerUpChange(PowerUp powerUp)
    {
        if (powerUp != null)
        {
            _hasPowerUp = true;
            _ai.StartCoroutine(WaitToDeployPowerUp());
        }
        else
        {
            _hasPowerUp = false;
        }
    }

    private IEnumerator WaitToDeployPowerUp()
    {
        yield return new WaitForSeconds(Random.Range(_minMaxPowerUpUsageTime.x, _minMaxPowerUpUsageTime.y));
        
        _runnerPowerUpManager.UsePowerUp();
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
