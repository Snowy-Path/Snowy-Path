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
        int ammo = gun.ammo;
        int maxMagazineCapacity = gun.maxMagazineCapacity;
        int maxAmmo = gun.maxAmmo;
        UpdateAmmoCount(ammo, maxMagazineCapacity, maxAmmo);
    }


    /// <summary>
    /// Update the HUD Ammo Counter
    /// </summary>
    /// <param name="currentAmmo"></param>
    /// <param name="maxmagazineCapacity"></param>
    private void UpdateAmmoCount(int currentAmmo, int maxmagazineCapacity, int maxAmmo)
    {
        AmmoText.text = currentAmmo + "/" + maxmagazineCapacity + " MaxAmmo=" + maxAmmo;
    }
}
