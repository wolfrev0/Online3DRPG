using UnityEngine;

public class EnemyChasing : StateMachineBehaviour
{
    Enemy _enemy;
    NavMeshAgent _nma;

    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (_enemy == null)
        {
            _enemy = animator.GetComponent<Enemy>();
            _nma = animator.GetComponent<NavMeshAgent>();
        }
        _nma.speed = 2.5f;
    }

    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (animator.IsInTransition(0))
            return;
        if (_enemy.hasTarget)
            _nma.destination = _enemy.mainTarget.transform.position;
        else
            animator.SetBool("Chase", false);
    }

    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        _nma.speed = 0f;
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
