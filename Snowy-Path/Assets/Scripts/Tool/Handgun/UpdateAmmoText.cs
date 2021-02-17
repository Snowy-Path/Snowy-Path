using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine;
using UnityEngine.UI;

public class UpdateAmmoText : MonoBehaviour
{
    public Text AmmoText; //For HUD ammo count
    public RayCastGun gun;
    void Update()
    {
        float ammo = gun.ammo;
        float maxMagazineCapacity = gun.maxMagazineCapacity;
        UpdateAmmoCount(ammo, maxMagazineCapacity);
    }


    /// <summary>
    /// Update the HUD Ammo Counter
    /// </summary>
    /// <param name="currentAmmo"></param>
    /// <param name="maxmagazineCapacity"></param>
    private void UpdateAmmoCount(float currentAmmo, float maxmagazineCapacity)
    {
        AmmoText.text = currentAmmo + "/" + maxmagazineCapacity;
    }
}
