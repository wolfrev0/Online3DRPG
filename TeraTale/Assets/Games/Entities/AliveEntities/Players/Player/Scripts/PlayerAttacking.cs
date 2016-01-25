using System.Collections;
using UnityEngine;

public class PlayerAttacking : StateMachineBehaviour
{
    Player _player;
    Animator _animator;
    Coroutine _attackStackTimer;

    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (_player == null)
        {
            _player = animator.GetComponent<Player>();
            _animator = animator;
        }

        int attackStack = animator.GetInteger("BaseAttackStack") + 1;
        if (attackStack > 2)
            attackStack = 0;
        animator.SetInteger("BaseAttackStack", attackStack);
        if (_attackStackTimer != null)
            _player.StopCoroutine(_attackStackTimer);
        _attackStackTimer = _player.StartCoroutine(ResetAttackStack());
        //Stack Overflow
    }

    IEnumerator ResetAttackStack()
    {
        yield return new WaitForSeconds(3.0f);
        _animator.SetInteger("BaseAttackStack", 0);
    }

    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (animator.IsInTransition(0))
            return;
        _player.HandleInput();
    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        _player.StopAttack();
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