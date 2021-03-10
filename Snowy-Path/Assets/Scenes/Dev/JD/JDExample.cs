using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Vector3.ProjectOnPlane - example

// Generate a random plane in xy. Show the position of a random
// vector and a connection to the plane. The example shows nothing
// in the Game view but uses Update(). The script reference example
// uses Gizmos to show the positions and axes in the Scene.

public class JDExample : MonoBehaviour {

    public Vector3 up;

    private void OnDrawGizmos() {
        RaycastHit hit;
        Ray ray = new Ray(transform.position, Vector3.down);
        if (Physics.Raycast(ray, out hit, 10f)) {

            Gizmos.color = Color.green;
            Gizmos.DrawLine(transform.position, hit.point);

            Gizmos.color = Color.red;
            Gizmos.DrawLine(hit.point, hit.point + hit.normal);

            Vector3 point = hit.point + hit.normal * 0.9f;
            Gizmos.DrawSphere(point, 0.1f);

            Vector3 projection = Vector3.ProjectOnPlane(point - hit.point + hit.normal, hit.normal);

            Gizmos.color = Color.cyan;
            //Vector3 v = Vector3.Cross(hit.normal, up).normalized;
            //Gizmos.DrawLine(hit.point + hit.normal, hit.point + hit.normal + v);

            Gizmos.DrawLine(hit.point + hit.normal, hit.point + hit.normal + projection);
            Gizmos.color = Color.blue;
            Gizmos.DrawLine(hit.point + hit.normal, hit.point + projection);

        }
    }
}