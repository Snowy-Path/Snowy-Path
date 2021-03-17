﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// Component holding a ScriptableObject of type Cloth.
/// </summary>
public class PickableCloth : MonoBehaviour {
    
    public Cloth scriptableCloth;
    private Inventory inventory;

    /// <summary>
    /// Retrieves the player Inventory script as a reference.
    /// </summary>
    private void Start() {
        inventory = FindObjectOfType<Inventory>();
    }

    /// <summary>
    /// It looks for the first Inventory object in scene and change the Cloth inside the Inventory script to the one stored here.
    /// Normaly, there is only a single one Inventory script in the scene.
    /// Must be used exclusively when interacting with this component's prefab/gameobject.
    /// </summary>
    public void ChangeClothInInventory() {
        inventory.ChangeCloth(scriptableCloth);
    }

}
