﻿using UnityEngine;

/// <summary>
/// Apply damage to player that enters the Collider.
/// The attack animation must disable/enable/disable the collider (at least) when the animation is running.
/// </summary>
[RequireComponent(typeof(Collider))]
public class EnnemyAttack : MonoBehaviour {

    [Tooltip("Damages dealt to player.")]
    [Min(0)]
    [SerializeField]
    public int attackDamage = 1;

    [Tooltip("Percentage reduction to the current player's temperature.")]
    [Range(0, 1)]
    [SerializeField]
    public float temperaturePercentageDamage = 0.1f;

    [Tooltip("Percentage reduction to the current player's durability.")]
    [Range(0, 1)]
    [SerializeField]
    public float clothPercentageDamage = 0.1f;

    [SerializeField]
    public bool instantKill = false;

    /// <summary>
    /// When another colliders collides, it verifies if it is tagged "Player" and then apply damage to it.
    /// </summary>
    /// <param name="other"></param>
    private void OnTriggerEnter(Collider other) {
        if (other.CompareTag("Player")) {
            if (instantKill) {
                other.GetComponent<GenericHealth>().Hit(other.GetComponent<GenericHealth>().maxHealth);
            } else {
                other.GetComponent<GenericHealth>().Hit(attackDamage);
            }
            other.GetComponent<Temperature>().ReduceCurrentTemperatureWithPercentage(temperaturePercentageDamage);
            other.GetComponent<Inventory>().ReduceClothDurabilityPercentage(clothPercentageDamage);
        }
    }

}
