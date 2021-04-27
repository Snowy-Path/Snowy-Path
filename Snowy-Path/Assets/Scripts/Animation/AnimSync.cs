using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimSync : StateMachineBehaviour {
    public bool rightHand = false;
    private Animator otherAnimator;
    private bool enteredByForce = false;

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {

        if (otherAnimator == null ) {
            if (rightHand)
                otherAnimator = GameObject.Find("LHand").GetComponent<Animator>();
            else
                otherAnimator = GameObject.Find("RHand").GetComponent<Animator>();
        }

        if (enteredByForce)
            return;

        AnimatorStateInfo animationState = otherAnimator.GetCurrentAnimatorStateInfo(0);
        if (animationState.IsName("Run")) {
            AnimatorClipInfo[] otherMotionClip = otherAnimator.GetCurrentAnimatorClipInfo(0);
            float syncTime = otherMotionClip[0].clip.length * animationState.normalizedTime;
            animator.Play("Run", 0, animationState.normalizedTime);
        }
        else if (animationState.IsName("Basic Motion")) {
            AnimatorClipInfo[] otherMotionClip = otherAnimator.GetCurrentAnimatorClipInfo(0);
            float syncTime = otherMotionClip[0].clip.length * animationState.normalizedTime;
            animator.Play("Basic Motion", 0, animationState.normalizedTime);
            enteredByForce = true;
        }
        Debug.Log( "Debug");

    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    //override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    
    //}

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
        enteredByForce = false;
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
