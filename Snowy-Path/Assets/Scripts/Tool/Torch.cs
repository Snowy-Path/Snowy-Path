using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// Script that manage the base attack animation and the damage dealing to ennemies.
/// It requires a Collider component with "IsTrigger" set to true.
/// </summary>
[RequireComponent(typeof(Collider))]
public class Torch : MonoBehaviour {

    [Min(0)]
    [Tooltip("Damage dealt while attacking. Applied to each ennemy detected by the Torch box collider.")]
    public int attackDamage = 1;

    [Tooltip("Hands animator. Allows this script to trigger the attack animation.")]
    public Animator animator;

    /// <summary>
    /// The main interaction of the torch.
    /// Perform the attack method only once.
    /// </summary>
    /// <param name="context"></param>
    public void MainInteraction(InputAction.CallbackContext context) {
        if (context.phase == InputActionPhase.Performed) {
            PerformAttack();
        }
    }

    /// <summary>
    /// The attack method. It starts the base attack animation.
    /// The animation itself MUST manage the box collider enabling/disabling.
    /// </summary>
    private void PerformAttack() {
        animator.SetTrigger("BaseAttack");
    }

    /// <summary>
    /// This method is active when the collider is enabled. Since it is enabled from the animator, it can only detected other collider when the animation is running.
    /// It is called for each collider detected. And it verifies for each collider if it was an ennemy. If so, it applies damage.
    /// </summary>
    /// <param name="other"></param>
    private void OnTriggerEnter(Collider other) {
        if (other.CompareTag("Ennemy")) {
            other.GetComponent<GenericHealth>().Hit(attackDamage);
        }
    }

}
