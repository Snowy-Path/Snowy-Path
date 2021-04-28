using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AmmoPile : MonoBehaviour
{
    public void ReloadGun() {
        var hands = FindObjectOfType<HandController>();
        if(hands != null)
        {
            var gun = hands.gun.GetComponent<Gun>();
            if (gun != null)
            {
                if (gun.ammunitionInInventory < gun.ammunitionInventoryLimit)
                {
                    gun.ammunitionInInventory = gun.ammunitionInventoryLimit;
                    //Destroy(this.gameObject);
                }
            }
        }
    }
}
