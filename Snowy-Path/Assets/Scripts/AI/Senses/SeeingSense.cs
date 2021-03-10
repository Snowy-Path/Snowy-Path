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

        agent.IsSeeingPlayer = (hits.Length == 1);

        if (agent.IsSeeingPlayer) {
            agent.LastPosition = other.transform.position;
        }

        Debug.DrawRay(ray.origin, ray.direction * length, agent.IsSeeingPlayer ? Color.green : Color.red);
    }

    private void OnTriggerExit(Collider other) {
        agent.IsSeeingPlayer = false;
    }

}
