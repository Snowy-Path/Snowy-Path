using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AmmoPile : MonoBehaviour
{
    public void ReloadGun() {
        var gun = FindObjectOfType<RayCastGun>();
        if (gun)
            gun.ReloadMax();
    }
}
