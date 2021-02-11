using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeatSource : MonoBehaviour
{
    public float radius = 10f;

    public bool showDebug = false;

    void Start()
    {
        // Initialize the object that will contain our trigger collider
        GameObject child = new GameObject("range");
        child.tag = "HeatSource";
        child.transform.parent = transform;
        child.transform.localPosition = Vector3.zero;

        // Initialize the collider
        SphereCollider sphereCollider = child.AddComponent<SphereCollider>();
        sphereCollider.isTrigger = true;
        sphereCollider.radius = radius;

        // Initialize a static rigidbody
        Rigidbody rigidbody = child.AddComponent<Rigidbody>();
        rigidbody.isKinematic = true;
    }

    void OnDrawGizmos()
    {
        if (showDebug) {
            Gizmos.color = new Color(1, 1, 1, 0.25f);
            Gizmos.DrawSphere(transform.position, radius);
            Gizmos.DrawWireSphere(transform.position, radius);
        }
    }
}
