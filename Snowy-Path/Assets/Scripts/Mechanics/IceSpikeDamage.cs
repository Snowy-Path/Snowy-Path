using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 
/// </summary>
public class IceSpikeDamage : MonoBehaviour {

    [Tooltip("Damages dealt to player.")]
    [Min(0)]
    public int attackDamage = 1;

    [Tooltip("Percentage reduction to the current player's temperature.")]
    [Range(0, 1)]
    public float temperaturePercentageDamage = 0.1f;

    [Tooltip("Percentage reduction to the current player's durability.")]
    [Range(0, 1)]
    public float clothPercentageDamage = 0.1f;

    /// <summary>
    /// Called when an ice spike (particle) collides with anything. If the other collider is tagged "Player", it will apply damages to the player.
    /// </summary>
    /// <param name="other"></param>
    void OnParticleCollision(GameObject other) {
        if (other.tag == "Player") {
            Debug.Log(other.tag);
            other.GetComponent<GenericHealth>().Hit(attackDamage);
            other.GetComponent<Temperature>().ReduceCurrentTemperatureWithPercentage(temperaturePercentageDamage);
            other.GetComponent<Inventory>().ReduceClothDurabilityPercentage(clothPercentageDamage);
        }
    }
}