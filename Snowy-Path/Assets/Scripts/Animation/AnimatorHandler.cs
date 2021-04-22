using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimatorHandler : MonoBehaviour {
    public Animator leftHand;
    public Animator rightHand;

    private void Update() {
        var state = leftHand.GetCurrentAnimatorStateInfo(0);
        bool animLock = state.tagHash == Animator.StringToHash("lock");
        leftHand.SetBool("Locked", animLock);
    }
}
