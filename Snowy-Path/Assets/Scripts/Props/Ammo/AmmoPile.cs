using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AmmoPile : MonoBehaviour {

    [SerializeField]
    private FMODUnity.StudioEventEmitter m_bulletPickUpEmitter;

    public void ReloadGun() {
        var hands = FindObjectOfType<HandController>();
        if (hands != null) {
            var gun = hands.gun.GetComponent<Gun>();
            if (gun != null) {
                if (gun.ammunitionInInventory < gun.ammunitionInventoryLimit) {
                    gun.ammunitionInInventory = gun.ammunitionInventoryLimit;
                    m_bulletPickUpEmitter.Play();
                    if (FindObjectOfType<HandController>()?.CurrentTool == (IHandTool)gun)
                    {
                        if (gun.ammoLoaded <= 0 && !gun.IsBusy)
                        {
                            gun.Reload();
                        }
                    }
                }
            }
        }
    }
}
