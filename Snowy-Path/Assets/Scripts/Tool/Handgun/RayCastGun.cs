﻿using UnityEngine;
using System.Collections;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class RayCastGun : Tool
{

    public float damageDealt = 1;
    public float fireRate = 0.25f;
    public float groupfire = 0.001f;
    public float range = 50f;                                        
    public float reloadingTime;
    public float projectileDispersion;
    

    public Transform gunEnd; //Reference to the gun end object, marking the muzzle location of the gun   
    private Camera fpsCam; //Reference to camera                                                
    private WaitForSeconds shotDuration = new WaitForSeconds(0.07f);// Determines how long time line will remain visible    
    private LineRenderer laserLine; //Visual effect                                        
    private float nextFire; //time between shooting
    private float timeBetweenShooting = 0.07f;
    private Vector3 forwardVector;
    public int projectileShot;
    public int ammo;
    private int currentMagazineCapacity = 0;
    
    public int maxAmmo; //ammo carried by the Player
    public int maxMagazineCapacity;
    public int projectilePerShot;
    private bool reloading, readyToShoot;



    void Start()
    {   
        //Get references of components linerenderer and camera
        laserLine = GetComponent<LineRenderer>();
        fpsCam = GetComponentInParent<Camera>();

        //Set ammo at full capacity
        ammo = maxMagazineCapacity;
        readyToShoot = true;
    }


    void Update()
    {
        Keyboard keyboard = Keyboard.current;
        
        //If the Player press fire button, isn't shooting or reloading and have ammo left
        if (keyboard.eKey.wasPressedThisFrame && Time.time > nextFire && !reloading && ammo > 0 && readyToShoot)
        {
            //Start shooting method
            projectileShot = projectilePerShot;
            MainInteraction();
            //counting ammo
            ammo--;
        }
        //If the Player press reload button
        if (keyboard.fKey.wasPressedThisFrame && maxAmmo>0)
        {
            //Start reloading method
            SecondaryInteraction();
        }


    }

    /// <summary>
    /// Visual effect of a laser line
    /// </summary>
    /// <returns></returns>
    private IEnumerator ShotEffect()
        {
            //Enable laserline effect during shotDuration
            laserLine.enabled = true;
            yield return shotDuration;
            laserLine.enabled = false;
        }

    /// <summary>
    /// Shooting method using Raycast, invoked as many times as there are projectiles per shot
    /// </summary>
    private new void MainInteraction()
    {

        readyToShoot = false;
        // Update the time when the player can fire next
        nextFire = Time.time + fireRate;
        //Start ShotEffect coroutine to turn laser line on and off
        StartCoroutine(ShotEffect());
        // Create a vector at the center of our camera's viewport
        Vector3 rayOrigin = fpsCam.ViewportToWorldPoint(new Vector3(0.5f, 0.5f, 0.0f));

        RaycastHit hit;

        //Set the start position for visual effect to the position of gunEnd
        laserLine.SetPosition(0, gunEnd.position);


        //Spread effective if several projectiles
        if (projectileShot > 1)
        {

            //calculate random angle within unity circle for dispersion
            forwardVector = Vector3.forward;
            float deviation = Random.Range(0f, projectileDispersion);
            float angle = Random.Range(0f, 360f);
            forwardVector = Quaternion.AngleAxis(deviation, Vector3.up) * forwardVector;
            forwardVector = Quaternion.AngleAxis(angle, Vector3.forward) * forwardVector;
            forwardVector = fpsCam.transform.rotation * forwardVector;
        }

        else
        {
            forwardVector = fpsCam.transform.forward;

        }


        //If the ray hit something
        if (Physics.Raycast(rayOrigin, forwardVector, out hit, range))
        {
        //Set the corresponding endline point
            laserLine.SetPosition(1, hit.point );
        }
        else
        {
        //If we did not hit anything, set the end of the line to a position directly in front of the camera at the distance of weaponRange
            laserLine.SetPosition(1, rayOrigin + ((fpsCam.transform.forward) * range));
        }
    
        //Counting projectiles
        projectileShot--;
        //Set player reaady to shot during time between shooting
        Invoke("ResetShot", timeBetweenShooting);
        
        //Shoot again if multiple projectiles
        if (projectileShot > 1 && ammo > 0)
            Invoke("MainInteraction", groupfire);
        //Update currentmagazinecapacity
        currentMagazineCapacity = maxMagazineCapacity - ammo;
    }

    /// <summary>
    /// Reload weapon if enough Ammo in inventory
    /// </summary>
    private new void SecondaryInteraction()
    {
        if (maxAmmo >= 0) 
        {          
            reloading = true;
            //Reload weapon during reloadingTime
            Invoke("ReloadFinished", reloadingTime);
            currentMagazineCapacity = maxMagazineCapacity - ammo;
        }

    }

    /// <summary>
    /// Set Player ready to shot again
    /// </summary>
    private void ResetShot()
    {
        readyToShoot = true;
    }

    /// <summary>
    /// Reload the weapon at max magazine capacity if enough max ammo
    /// </summary>
    private void ReloadFinished()
    {
        //Calculate cuurentMagazineCapacity
        currentMagazineCapacity = maxMagazineCapacity - ammo;
        
        //If the player have more ammo than the size of magazine
        if (maxAmmo >= maxMagazineCapacity)
        {
            //Full reload
            maxAmmo -= currentMagazineCapacity;
            ammo = maxMagazineCapacity;
            reloading = false;
            currentMagazineCapacity = 0;
        }

        if (maxAmmo < maxMagazineCapacity)
        {
            //Else, it depends on the current magazine capacity
            if ( maxAmmo > currentMagazineCapacity)
            {
                ammo = maxMagazineCapacity;
                maxAmmo -= currentMagazineCapacity;
                reloading = false;
            }

            else
            {
                ammo = currentMagazineCapacity + maxAmmo;
                maxAmmo = 0;
                reloading = false;
                
            }

        }
        

    }



}