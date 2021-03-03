using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class VRScope : MonoBehaviour, IHandTool
{
    [Tooltip("Hands animator. Allows this script to trigger the look animation.")]
    public Animator animator;

    public EToolType ToolType => throw new System.NotImplementedException();

    public void StartPrimaryUse() {
        if (animator.GetCurrentAnimatorStateInfo(0).IsName("Idle")) { // Without this line, the animation can be triggered WHILE playing. Meaning it will repeat again & again.
            animator.SetTrigger("BaseAttack");
        }
    }

    public void CancelPrimaryUse() {
        throw new System.NotImplementedException();
    }

    public void SecondaryUse() {
        throw new System.NotImplementedException();
    }

    public void ToggleDisplay(bool display) {
        throw new System.NotImplementedException();
    }
}
