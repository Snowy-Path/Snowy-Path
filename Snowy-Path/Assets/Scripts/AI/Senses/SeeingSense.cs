using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SeeingSense : MonoBehaviour {

    public string tagTarget = "Player";
    public WolfController agent;
    public LayerMask layers;

    private void OnTriggerStay(Collider other) {

        if (!other.CompareTag(tagTarget)) {
            return;
        }

        Vector3 origin = transform.position;
        Vector3 destination = other.transform.position;
        Vector3 direction = destination - origin;
        float length = direction.magnitude;
        direction.Normalize();

        Ray ray = new Ray(origin, direction);

        RaycastHit[] hits = Physics.RaycastAll(ray, length, layers);

        bool isSeeingPlayer = false;

        if (hits.Length == 1) {
            isSeeingPlayer = true;
        }

        if (isSeeingPlayer) {
            agent.Target = other.transform;
        } else {
            agent.Target = null;
        }

        Debug.DrawRay(ray.origin, ray.direction * length, isSeeingPlayer ? Color.green : Color.red);
    }

    private void OnTriggerExit(Collider other) {
        agent.Target = null;
    }

}
