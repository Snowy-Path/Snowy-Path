using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeatSource : MonoBehaviour
{
    public float radius = 10f;

    public bool showDebug = false;

    SphereCollider sphereCollider;

    GameObject child;
    void Awake()
    {
        // Initialize the object that will contain our trigger collider
        child = new GameObject("range");
        child.tag = "HeatSource";
        child.transform.parent = transform;
        child.transform.localPosition = Vector3.zero;
        child.layer = 2;

        // Initialize the collider
        sphereCollider = child.AddComponent<SphereCollider>();
        sphereCollider.enabled = false;
        sphereCollider.isTrigger = true;
        sphereCollider.radius = radius;
    }

    private void OnEnable()
    {
        sphereCollider.enabled = true;
    }

    private void OnDisable()
    {
        sphereCollider.enabled = false;
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
