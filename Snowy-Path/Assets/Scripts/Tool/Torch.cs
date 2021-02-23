using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Torch : MonoBehaviour
{

    [Min(0)]
    public int attackDamage = 1;

    [Min(0)]
    public float attackRecovery = 0.2f;

    public Animator animator;

    public void MainInteraction(InputAction.CallbackContext context) {
        if (context.phase == InputActionPhase.Performed) {
            PerformAttack();
        }
    }

    private void PerformAttack() {
        animator.SetTrigger("BaseAttack");
    }

    private void OnTriggerEnter(Collider other) {

        if (other.CompareTag("Ennemy")) {
            other.GetComponent<GenericHealth>().Hit(attackDamage);
        }

    }

}
