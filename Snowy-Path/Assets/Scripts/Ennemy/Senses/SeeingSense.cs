using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SeeingSense : MonoBehaviour {

    public string tagTarget = "Player";
    public GameObject agent;

    private void OnTriggerStay(Collider other) {
        
        //Guard
        if (!other.CompareTag(tagTarget)) {
            return;
        }

        GameObject target = other.gameObject;
        Vector3 agentPos = agent.transform.position;
        Vector3 targetPos = target.transform.position;
        Vector3 direction = targetPos - agentPos;

        float length = direction.magnitude;
        direction.Normalize();
        Ray ray = new Ray(agentPos, direction);

        RaycastHit[] hits = Physics.RaycastAll(ray, length);

        //Guard
        if (hits.Length == 0) {
            return;
        }

        //Find closest
        int closest = 0;
        for (int i = 1; i < hits.Length; i++) {
            if (hits[i].distance < hits[closest].distance) {
                closest = i;
            }
        }

        //If closest if Player, then we see him
        if (hits[closest].collider.CompareTag(tagTarget)) {
            gameObject.GetComponent<WolfController>().isSeeingPlayer = true;
            gameObject.GetComponent<WolfController>().target = hits[closest].collider.transform;
        }
        //Else, we do not see him
        else {
            gameObject.GetComponent<WolfController>().isSeeingPlayer = false;
            gameObject.GetComponent<WolfController>().target = null;
        }

        Debug.DrawRay(ray.origin, ray.direction * length);
    }

}
