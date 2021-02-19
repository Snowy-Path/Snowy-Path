using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickableCloth : MonoBehaviour {
    
    public Cloth scriptableCloth;

    public void ChangeClothInInventory() {
        Inventory inventory = FindObjectOfType<Inventory>();
        inventory.ChangeCloth(scriptableCloth);
    }

}
