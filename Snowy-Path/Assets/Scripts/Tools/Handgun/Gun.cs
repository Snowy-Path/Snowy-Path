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

    public int projectilePerShot = 5;
    public int range = 1000;
    public float spread = 1;
    public int damage = 0;
    public float fireRate = 0;


    [Header("Settings")]
    public LayerMask ignoredLayers;
    public GameObject impactPrefab;
    private Camera fpsCamera;

    [Header("AI")]
    // AI script for the hearing sense
    public HearingSenseEmitter emitter;
    public UnityEvent onEquip;

    private bool shootReady = true;


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

        for (int i = 0; i < projectilePerShot; i++) {

            float x = Random.Range(-spread, spread);
            float y = Random.Range(-spread, spread);
            float z = Random.Range(-spread, spread);
            Vector3 randDirection = new Vector3(x, y, z);

            //If the ray hit something
            if (Physics.Raycast(fpsCamera.transform.position, fpsCamera.transform.forward + randDirection, out hit, range, ignoredLayers)) {

                if (impactPrefab) {
                    GameObject impactGo = Instantiate(impactPrefab, hit.point, Quaternion.Euler(hit.normal));
                    Destroy(impactGo, 1f);
                }

                //Set the corresponding endline point
                if (hit.transform.CompareTag("Ennemy")) {
                    IEnnemyController ennemyController = hit.transform.GetComponent<IEnnemyController>();
                    if (ennemyController != null) {
                        ennemyController.Hit(EToolType.Pistol, damage);
                    }
                }
            }
        }

        ammoLoaded--;
        handAnimator.SetTrigger("Shoot");
        onShoot.Invoke();
        StartCoroutine(FireRateCoroutine());
    }

    IEnumerator FireRateCoroutine() {
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
        //if (ammunitionInInventory > 0) {
        //    Reload();
        //}
    }

    public void ToggleDisplay(bool display) {
        gameObject.SetActive(display);
        if (display) {
            onEquip.Invoke();
        }
    }
}
