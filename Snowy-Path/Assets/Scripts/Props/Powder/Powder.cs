using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Powder : MonoBehaviour {

    public void ActivatePowderEffects() {
        PowderController powderController = FindObjectOfType<PowderController>();
        powderController.ActivatePowderEffects();
    }

}
