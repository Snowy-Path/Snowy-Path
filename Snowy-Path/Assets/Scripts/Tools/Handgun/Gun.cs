using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Gun : MonoBehaviour, IHandTool {
    public EToolType ToolType => EToolType.Pistol;

    public bool IsBusy { get; set; }
    public Animator handAnimator { get; set; }

    public UnityEvent onShoot;

    [Header("Inventory")]
    public int ammunitionInInventory = 0;
    public int ammunitionInventoryLimit = 5;


    [Header("Gun")]
    public int ammoLoaded = 0;
    public int ammoLoadedLimit = 5;
    public float reloadingTime = 4;

    public int range = 1000;
    public int damage = 0;
    public float fireRate = 0;
    private bool shootReady = true;


    [Header("Settings")]
    public LayerMask ignoredLayers;
    private Camera fpsCamera;


    [Header("AI")]
    // AI script for the hearing sense
    public HearingSenseEmitter emitter;
    public UnityEvent onEquip;

    private void Awake() {
        fpsCamera = GetComponentInParent<Camera>();
    }

    public void StartPrimaryUse() {
        bool hasShot = false;
        if (ammoLoaded > 0 && shootReady && !IsBusy) {
            Shoot();
            hasShot = true;
        }
        if (ammoLoaded <= 0 && ammunitionInInventory > 0) {
            if (hasShot)
                Invoke(nameof(Reload), 0.3f);
            else
                Reload();
        }
    }

    /// <summary>
    /// Triggered when the player hit primary use
    /// </summary>
    private void Shoot() {
        RaycastHit hit;
        //If the ray hit something
        if (Physics.Raycast(fpsCamera.transform.position, fpsCamera.transform.forward, out hit, range, ignoredLayers)) {
            //Set the corresponding endline point
            if (hit.transform.CompareTag("Ennemy")) {
                IEnnemyController ennemyController = hit.transform.GetComponent<IEnnemyController>();
                if (ennemyController != null) {
                    ennemyController.Hit(EToolType.Pistol, damage);
                }
            }
        }
        handAnimator.SetTrigger("Shoot");
        onShoot.Invoke();
        ammoLoaded--;
        StartCoroutine(fireRateCoroutine());
    }

    IEnumerator fireRateCoroutine() {
        shootReady = false;
        yield return new WaitForSeconds(fireRate);
        shootReady = true;
    }

    /// <summary>
    /// Triggered when the player hit secondary use or don't have any ammo
    /// </summary>
    private void Reload() {

        handAnimator.SetTrigger("Reload");

        int ammoToReload = ammoLoadedLimit - ammoLoaded;

        if (ammoToReload < ammunitionInInventory) {
            ammunitionInInventory -= ammoToReload;
            ammoLoaded += ammoToReload;
        }
        else {
            ammoLoaded += ammunitionInInventory;
            ammunitionInInventory = 0;
        }

        //Reload weapon during reloadingTime
        IsBusy = true;
        Invoke(nameof(ReloadFinished), reloadingTime);
    }

    private void ReloadFinished() {
        IsBusy = false;
    }

    public void CancelPrimaryUse() {
    }

    public void SecondaryUse() {
        if (ammunitionInInventory > 0) {
            Reload();
        }
    }

    public void ToggleDisplay(bool display) {
        gameObject.SetActive(display);
        if (display) {
            onEquip.Invoke();
        }
    }
}
