using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Campfire : MonoBehaviour {
    [Header("Campfire ID")]
    [Tooltip("Id can be generate by rightclick on the scripts and generateID")]
    // The ID of the Saveable entity that will link this object with the data saved
    [SerializeField] private string id = string.Empty;

    public string Id => id;

    [Header("Campfire Tweaking")]

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

    public LightAndFogAsset lightForThisCampfire;

    private HeatSource heatSource;

    private float extinctionFireTimer = 0f;

    // Stopping timer when fire is not active
    private bool isFireActive = false;

    // The ID must be generated for it to be saved properly
    [ContextMenu("Generate Id")]
    private void GenerateId() => id = Guid.NewGuid().ToString();


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
        PlayerCampfireSave playerCampfireSave = FindObjectOfType<PlayerCampfireSave>();
        playerCampfireSave.LastCampfireId = this.id;
        SceneSave sceneSave = FindObjectOfType<SceneSave>();
        if (sceneSave)
            sceneSave.SceneName = this.gameObject.scene.name;
        heatSource.enabled = true;
        GenericHealth playerHealth = playerCampfireSave.gameObject.GetComponent<GenericHealth>();
        if (playerHealth != null) {
            playerHealth.FullHeal();
        }
        onIgnite.Invoke();

    }

    /// <summary>
    /// Call every method necessary when the campfire need to be extinguished
    /// </summary>
    internal void ExctinguishFire() {
        isFireActive = false;
        extinctionFireTimer = 0;
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
