using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[DisallowMultipleComponent]
public class AnimatorLock : MonoBehaviour {
    private Animator animator;

    private void Awake() {
        animator = GetComponent<Animator>();
    }

    private void Update() {
        var state = animator.GetCurrentAnimatorStateInfo(0);
        bool animLock = state.tagHash == Animator.StringToHash("lock");
        animator.SetBool("Locked", animLock);
    }
}
