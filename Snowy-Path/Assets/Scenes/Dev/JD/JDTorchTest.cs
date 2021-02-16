using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JDTorchTest : MonoBehaviour
{

    public GameObject fireFX;

    public void ToggleFire() {
        fireFX.SetActive(!fireFX.activeSelf);
    }
}
