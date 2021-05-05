﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimSync : MonoBehaviour {

    [SerializeField] Animator otherAnimator;
    [SerializeField] string motionStateName = "Basic Motion";
    [SerializeField] string runStateName = "Run";

    private Animator animator;
    private int lastStateHash = -1;

    private int motionStateHash;
    private int runStateHash;

    private void Start() {
        animator = GetComponent<Animator>();
        motionStateHash = Animator.StringToHash(motionStateName);
        runStateHash = Animator.StringToHash(runStateName);
    }

    private void Update() {
        AnimatorStateInfo state = animator.GetCurrentAnimatorStateInfo(0);
        if (lastStateHash != motionStateHash && lastStateHash != runStateHash) {
            if (state.shortNameHash == motionStateHash || state.shortNameHash == runStateHash) {

                AnimatorStateInfo otherState = otherAnimator.GetCurrentAnimatorStateInfo(0);
                if (otherState.shortNameHash == motionStateHash) {
                    animator.CrossFade(motionStateHash, .15f, 0, otherState.normalizedTime );
                    //animator.Play(motionStateHash, 0, otherState.normalizedTime);
                }
                else if (otherState.shortNameHash == runStateHash) {
                    animator.CrossFade(runStateHash, .15f, 0, otherState.normalizedTime );
                    //animator.Play(runStateHash, 0, otherState.normalizedTime);
                }
            }
        }
        lastStateHash = state.shortNameHash;

        //if (UnityEngine.InputSystem.Keyboard.current.oKey.wasPressedThisFrame) {
        //    var player = FindObjectOfType<PlayerController>();
        //    var health = player.GetComponent<GenericHealth>();
        //    health.Hit(100);
        //}
    }

    public void PlayDeathAnimation() {
        animator.SetTrigger("Death");
    }

    public void ResetAnimator() {
        animator.Play("Basic Motion");
    }

}
