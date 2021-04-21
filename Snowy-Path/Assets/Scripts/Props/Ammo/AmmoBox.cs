using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AmmoBox : MonoBehaviour
{
    public void ReloadGun() {
        var gun = FindObjectOfType<RayCastGun>();
        if (gun)
            gun.ReloadOneAmmo();
    }
}
