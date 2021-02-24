using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Campfire : MonoBehaviour
{
    [Tooltip("Time for the fire to extinguish")]
    public float extinctionTime = 0f;

    [Tooltip("Radius of the extinction range")]
    public float radius = 0f;

    [Header("UnityEvents callbacks")]

    [Tooltip("Callbacks called when the fire ignites.")]
    public UnityEvent onIgnite;

    [Tooltip("Callbacks called when the fire extinguish.")]
    public UnityEvent onExtinguish;

    public bool showDebug = false;

    private HeatSource heatSource;

    private float extinctionFireTimer = 0f;

    // Stopping timer when fire is not active
    private bool isFireActive = false;


    private void Start() {
        heatSource = GetComponent<HeatSource>();
        heatSource.enabled = false;
    }

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
        isFireActive = true;
        onIgnite.Invoke();
        heatSource.enabled = true;
    }

    /// <summary>
    /// Call every method necessary when the campfire need to be extinguished
    /// </summary>
    internal void ExctinguishFire() {
        isFireActive = false;
        onExtinguish.Invoke();
        heatSource.enabled = false;
    }

    void OnDrawGizmos() {
        if (showDebug) {
            Gizmos.color = new Color(1, 1, 1, 0.25f);
            Gizmos.DrawSphere(transform.position, radius);
            Gizmos.DrawWireSphere(transform.position, radius);
        }
    }
}
