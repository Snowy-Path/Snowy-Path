using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Torch : MonoBehaviour
{

    [Min(0)]
    public float attackRange = 1.0f; //Range from animation ???

    [Min(0)]
    public int attackDamage = 1;

    [Min(0)]
    public float attackRecovery = 0.2f;
    private float attackRecoveryTimer = 0f;

    [Min(0)]
    public float castAttack = 0.4f;

    public Animator animator;

    public void MaintInteraction(InputAction.CallbackContext context) {
        if (context.phase == InputActionPhase.Performed) {
            PerformAttack();
        }
    }

    private void PerformAttack() {
        animator.SetTrigger("Attack");
    }

    private void OnTriggerEnter(Collider other) {

        if (other.CompareTag("Ennemy")) {
            other.GetComponent<GenericHealth>().Hit(attackDamage);
        }

    }

}
