using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireDetector : MonoBehaviour {

    [SerializeField] Animator leftAnimator;
    [SerializeField] Animator rightAnimator;

    private void OnTriggerStay(Collider other) {
        Campfire campfire = other.GetComponent<Campfire>();
        if (campfire && campfire.isFireActive) {
            UpdateAnimators(true);
        }
    }

    private void OnTriggerExit(Collider other) {
        Campfire campfire = other.GetComponent<Campfire>();
        if (campfire && campfire.isFireActive) {
            UpdateAnimators(false);
        }
    }

    private void UpdateAnimators(bool campfire) {
        leftAnimator.SetBool("Campfire", campfire);
        rightAnimator.SetBool("Campfire", campfire);
    }
}
