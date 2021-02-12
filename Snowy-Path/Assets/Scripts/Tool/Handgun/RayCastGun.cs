using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class RayCastGun : Tool
{


    public float damageDealt = 1;                                            
    public float fireRate = 0.25f;
    public float groupfire = 0.001f;
    public float range = 50f;                                        
    public float hitForce = 100f;
    public float reloadingTime;
    public float projectileDispersion;
    public float timeBetweenShooting = 0.07f;
    private int projectileShot;

    public Transform gunEnd;
    public Text AmmoText;
    private Camera fpsCam;                                                
    private WaitForSeconds shotDuration = new WaitForSeconds(0.07f);    
    private LineRenderer laserLine;                                        
    private float nextFire;

    private int ammo; 
    public int maxMagazineCapacity, projectilePerShot;
    bool reloading, readyToShoot;



    void Start()
    {        
        laserLine = GetComponent<LineRenderer>();
        fpsCam = GetComponentInParent<Camera>();
        ammo = maxMagazineCapacity;
        readyToShoot = true;
    }


    void Update()
    {
        if (Input.GetButtonDown("Fire1") && Time.time > nextFire && !reloading && ammo > 0 && readyToShoot)
        {
            if (readyToShoot && !reloading && ammo > 0)
            {
                projectileShot = projectilePerShot;
                MainInteraction();
            }
        }

        if (Input.GetButtonDown("Fire2"))
        {
            SecondaryInteraction();
        }

        UpdateAmmoText(ammo, maxMagazineCapacity);

    }



    private IEnumerator ShotEffect()
        {

            laserLine.enabled = true;
            yield return shotDuration;
            laserLine.enabled = false;
        }

    private new void MainInteraction()
    {

        readyToShoot = false;
        nextFire = Time.time + fireRate;           
        StartCoroutine(ShotEffect());          
        Vector3 rayOrigin = fpsCam.ViewportToWorldPoint(new Vector3(0.5f, 0.5f, 0.0f));     
        RaycastHit hit;
        laserLine.SetPosition(0, gunEnd.position);
        float x = Random.Range(-projectileDispersion, projectileDispersion);
        float y = Random.Range(-projectileDispersion, projectileDispersion);
        Vector3 dispersion = new Vector3(x, y, 0);


    //Si le rayon touche qqch
        if (Physics.Raycast(rayOrigin, fpsCam.transform.forward, out hit, range))
        {               
            laserLine.SetPosition(1, hit.point);              
            ShootableBox health = hit.collider.GetComponent<ShootableBox>();               
            if (health != null)
            {                   
                health.Damage(damageDealt);
            }
               
            if (hit.rigidbody != null)
            {                    
                hit.rigidbody.AddForce(-hit.normal * hitForce);
            }
        }
        else
        {                
            laserLine.SetPosition(1, rayOrigin + (fpsCam.transform.forward * range) + dispersion);
        }
    
    
    ammo--;
    projectileShot--;
        
        Invoke("ResetShot", timeBetweenShooting);
    
        if (projectileShot > 0 && ammo > 0)
            Invoke("MainInteraction", groupfire);
    }

    private new void SecondaryInteraction()
    {
        reloading = true;
        Invoke("ReloadFinished", reloadingTime);
    }


    private void ResetShot()
    {
        readyToShoot = true;
    }

    private void ReloadFinished()
    {
        ammo = maxMagazineCapacity;
        reloading = false;
    }

    public void UpdateAmmoText(float currentAmmo, float maxAmmo)
    {
        AmmoText.text = currentAmmo + "/" + maxAmmo;
    }

}