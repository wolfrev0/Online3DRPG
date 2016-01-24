using System;
using UnityEngine;

public class EnemyAttacking : StateMachineBehaviour
{
    Enemy _enemy;

    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (_enemy == null)
            _enemy = animator.GetComponent<Enemy>();
        //Maybe all monsters are Generic Type...
        //if (animator.ishuman)
        //{
        //    var events = animator.getcurrentanimatorclipinfo(0)[0].clip.events;
        //    var targetpos = _enemy.target.transform.position;
        //    var targetrot = _enemy.target.transform.rotation;
        //    var attackbegintime = array.find(events, (animationevent ev) => { return ev.functionname == "attackbegin"; }).time;
        //    var attackendtime = array.find(events, (animationevent ev) => { return ev.functionname == "attackend"; }).time;
        //    animator.matchtarget(targetpos, targetrot, avatartarget.righthand, new matchtargetweightmask(vector3.one, 1f), attackbegintime, attackendtime);
        //}
    }

    //override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //}

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    //override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //}

    // OnStateMove is called right after Animator.OnAnimatorMove(). Code that processes and affects root motion should be implemented here
    //override public void OnStateMove(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
    //
    //}

    // OnStateIK is called right after Animator.OnAnimatorIK(). Code that sets up animation IK (inverse kinematics) should be implemented here.
    //override public void OnStateIK(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
    //
    //}
}