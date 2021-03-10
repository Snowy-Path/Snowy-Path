using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnnemyAttack : MonoBehaviour {

    [Tooltip("Damages dealt when player is detected")]
    public int attackDamage = 1;

    [Tooltip("Current temperature percentage.")]
    [Range(0, 1)]
    public float temperaturePercentageDamage = 0.1f;

    [Tooltip("Current durability percentage.")]
    [Range(0, 1)]
    public float clothPercentageDamage = 0.1f;

    private void OnTriggerEnter(Collider other) {
        if (other.CompareTag("Player")) {
            other.GetComponent<GenericHealth>().Hit(attackDamage);
            other.GetComponent<Temperature>().ReduceCurrentTemperatureWithPercentage(temperaturePercentageDamage);
            other.GetComponent<Inventory>().ReduceClothDurabilityPercentage(clothPercentageDamage);
        }
    }
}
