using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
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

    public MonoBehaviour[] lockingTools;
    public UnityEvent onAttack;


    [SerializeField]
    [FMODUnity.EventRef]
    private string m_attackEventPath;
    [SerializeField]
    private Transform m_attackEmissionPosition;
    private FMOD.Studio.EventInstance m_attackInstance;
    private FMOD.Studio.PARAMETER_ID m_attackID;

    private float m_rockHitValue = 0.0f;
    private float m_snowHitValue = 0.15f;
    private float m_iceHitValue = 0.25f;
    private float m_waterHitValue = 0.35f;
    private float m_woodHitValue = 0.45f;
    private float m_leavesHitValue = 0.55f;
    private float m_fleshHitValue = 0.65f;

    private bool attackLocked = false;

    private void Start() {
        m_attackInstance = FMODUnity.RuntimeManager.CreateInstance(m_attackEventPath);

        FMOD.Studio.EventDescription attackEventDesc;
        m_attackInstance.getDescription(out attackEventDesc);
        FMOD.Studio.PARAMETER_DESCRIPTION attackParametterDesc;
        attackEventDesc.getParameterDescriptionByIndex(0, out attackParametterDesc);
        m_attackID = attackParametterDesc.id;
    }

    private void Update() {
        m_attackInstance.set3DAttributes(FMODUnity.RuntimeUtils.To3DAttributes(m_attackEmissionPosition));
    }

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

        bool isBusy = false;
        foreach (var tool in lockingTools) {
            IHandTool iTool = tool.GetComponent<IHandTool>();
            if (iTool.IsBusy) {
                isBusy = true;
                break;
            }
        }

        if (!animator.GetCurrentAnimatorStateInfo(0).IsName("Attack") && attackLocked == false && !isBusy) {
            onAttack.Invoke();
            animator.SetTrigger("Attack");
            attackLocked = true;
            Invoke(nameof(ResetAttack), 0.3f);
        }
    }

    private void ResetAttack() {
        attackLocked = false;
    }

    private void PlayAttackSoundValue(float value) {
        m_attackInstance.setParameterByID(m_attackID, value);
        m_attackInstance.start();
    }

    /// <summary>
    /// This method is active when the collider is enabled. Since it is enabled from the animator, it can only detected other collider when the animation is running.
    /// It is called for each collider detected. And it verifies for each collider if it was an ennemy. If so, it applies damage.
    /// </summary>
    /// <param name="other"></param>
    private void OnTriggerEnter(Collider other) {
        if (other.CompareTag("Ennemy")) {
            other.GetComponent<IEnnemyController>().Hit(EToolType.Torch, attackDamage);
            PlayAttackSoundValue(m_fleshHitValue);
        } else if (other.CompareTag("Ice")) {
            PlayAttackSoundValue(m_iceHitValue);
        } else {
            Interactable inter = other.GetComponent<Interactable>();
            if (inter && inter.IsTorchInteractable) {
                inter.Interact();
                if (other.CompareTag("Campfire") || other.CompareTag("FireInteractable")) {
                    PlayAttackSoundValue(m_woodHitValue);
                } else {
                    PlayAttackSoundValue(m_rockHitValue);
                }
            }
        }

    }

}
