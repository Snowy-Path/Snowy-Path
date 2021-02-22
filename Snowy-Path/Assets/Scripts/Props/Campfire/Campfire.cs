using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Campfire : MonoBehaviour
{
    [Tooltip("Time for the fire to extinguish")]
    public float extinctionTime = 0;

    private float extinctionFireTimer = 0;

    public bool isFireActive = false;

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

    public void IgniteFire() {
        GetComponentInChildren<MeshRenderer>().materials[1].color = new Color(1, 0, 0);
        isFireActive = true;

    }

    void ExctinguishFire() {
        GetComponentInChildren<MeshRenderer>().materials[1].color = new Color(0,0,0);
        isFireActive = false;
    }

}
