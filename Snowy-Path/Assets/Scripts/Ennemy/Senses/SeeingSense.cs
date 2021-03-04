using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SeeingSense : MonoBehaviour {

    public string tagTarget = "Player";
    public WolfController agent;

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

        //Something is blocking the view
        if (hits.Length != 0) {
            return;
        }

        //If closest if Player, then we see him
        if (other.CompareTag(tagTarget)) {
            agent.isSeeingPlayer = true;
            agent.target = other.transform;
        }
        //Else, we do not see him
        else {
            agent.isSeeingPlayer = false;
            agent.target = null;
        }

        Debug.DrawRay(ray.origin, ray.direction * length, Color.green);
    }

}
