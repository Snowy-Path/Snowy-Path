using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CampfireZone : MonoBehaviour
{
    // The campfire main script
    Campfire campfire;
    private void Start() {

        // Getting the campfire script
        campfire = GetComponentInParent<Campfire>();

        // Initialize the collider
        SphereCollider sphereCollider = gameObject.AddComponent<SphereCollider>();
        sphereCollider.isTrigger = true;
        sphereCollider.radius = campfire.radius;
    }

    private void OnTriggerExit(Collider other) {
        if (other.CompareTag("Player")) {
            campfire.ExctinguishFire();
        }
    }

}
