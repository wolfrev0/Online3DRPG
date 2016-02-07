using UnityEngine;

public class PlayerRunning : StateMachineBehaviour
{
    Player _player;
    NavMeshAgent _nma;
    Animator _animator;

	override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (_player == null)
        {
            _player = animator.GetComponent<Player>();
            _nma = animator.GetComponent<NavMeshAgent>();
            _animator = animator;
        }
        _nma.speed = 4;
        _animator.SetBool("Run", true);
        InputHandler.instance.enabled = true;
    }
    
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (animator.IsInTransition(0))
            return;
        _player.FacingDirectionUpdate();
        if (_player.isArrived)
            _animator.SetBool("Run", false);
    }

    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        _nma.speed = 0;
        _nma.destination = _player.transform.position;
        _animator.SetBool("Run", false);
    }

    // OnStateMove is called right after Animator.OnAnimatorMove(). Code that processes and affects root motion should be implemented here
    //override public void OnStateMove(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
    //
    //}

    // OnStateIK is called right after Animator.OnAnimatorIK(). Code that sets up animation IK (inverse kinematics) should be implemented here.
    //override public void OnStateIK(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
    //
    //}
}
