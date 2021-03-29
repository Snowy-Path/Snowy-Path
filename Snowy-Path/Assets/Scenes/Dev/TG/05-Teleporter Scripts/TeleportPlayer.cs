using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeleportPlayer : MonoBehaviour
{
    public Vector3 teleportPosition = new Vector3(0,0,0);

    public void Teleport() {
        CharacterController charController = FindObjectOfType<CharacterController>();
        charController.enabled = false;
        charController.transform.position = teleportPosition;
        charController.enabled = true;
    }
}
