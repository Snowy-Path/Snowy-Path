using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimSync : MonoBehaviour {

    [SerializeField] Animator otherAnimator;
    [SerializeField] string motionState = "Basic Motion";
    [SerializeField] string runState = "Run";

    private Animator animator;
    private int lastStateHash = -1;

    private int motionStateHash;
    private int runStateHash;

    private void Start() {
        animator = GetComponent<Animator>();
        motionStateHash = Animator.StringToHash(motionState);
        runStateHash = Animator.StringToHash(runState);
    }

    private void Update() {
        AnimatorStateInfo state = animator.GetCurrentAnimatorStateInfo(0);
        if (lastStateHash != motionStateHash && lastStateHash != runStateHash) {
            if (state.IsName(motionState) || state.IsName(runState)) {

                AnimatorStateInfo otherState = otherAnimator.GetCurrentAnimatorStateInfo(0);
                if (otherState.IsName(motionState))
                    animator.Play(motionState, 0, otherState.normalizedTime);
                else if (otherState.IsName(runState))
                    animator.Play(runState, 0, otherState.normalizedTime);
            }
        }
        lastStateHash = state.shortNameHash;
    }
}
