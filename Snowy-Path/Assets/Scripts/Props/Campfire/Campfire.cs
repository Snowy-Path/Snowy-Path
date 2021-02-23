using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Campfire : MonoBehaviour
{

    [Tooltip("Time for the fire to extinguish")]
    public float extinctionTime = 0f;

    [Tooltip("Radius of the extinction range")]
    public float radius = 0f;

    private float extinctionFireTimer = 0f;

    // Stopping timer when fire is not active
    private bool isFireActive = false;

    public bool showDebug = false;

    private void Update() {

        if (isFireActive) {
            // Increase the extinction timer
            extinctionFireTimer += Time.deltaTime;

            // Process exctinction and reset timer if its value is higher than cooldown
            if (extinctionFireTimer > extinctionTime) {
                extinctionFireTimer = 0;

                ExctinguishFire();
            }
        }
    }

    /// <summary>
    /// Call every method necessary when interacting with the campfire
    /// </summary>
    public void IgniteFire() {
        GetComponentInChildren<MeshRenderer>().materials[1].color = new Color(1, 0, 0);
        isFireActive = true;
    }

    /// <summary>
    /// Call every method necessary when the campfire need to be exctinguished
    /// </summary>
    internal void ExctinguishFire() {
        GetComponentInChildren<MeshRenderer>().materials[1].color = new Color(0,0,0);
        isFireActive = false;
    }

    void OnDrawGizmos() {
        if (showDebug) {
            Gizmos.color = new Color(1, 1, 1, 0.25f);
            Gizmos.DrawSphere(transform.position, radius);
            Gizmos.DrawWireSphere(transform.position, radius);
        }
    }
}
