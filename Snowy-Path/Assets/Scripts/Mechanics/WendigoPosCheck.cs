using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WendigoPosCheck : MonoBehaviour {
    [SerializeField] Transform newWendigoPos;
    [SerializeField] float teleportDistance = 10;
    private bool triggered = false;

    private void OnTriggerEnter(Collider other) {
        WendigoFollow wendigo = FindObjectOfType<WendigoFollow>();
        if (wendigo) {
            if (other.CompareTag("Player") && !triggered) {
                if ((wendigo.transform.position - other.transform.position).magnitude > teleportDistance) {
                    Teleport(wendigo.transform, newWendigoPos.position);
                    wendigo.transform.position = newWendigoPos.position;
                }
                triggered = true;
            }
        }
        else
            Debug.LogError("Can't find Wendigo to teleport");
    }

    private void Teleport(Transform transform, Vector3 point) {
        transform.position = point;
    }

    private void OnDrawGizmos() {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, teleportDistance);
        if (newWendigoPos)
            Gizmos.DrawSphere(newWendigoPos.position, 0.5f);
    }
}

