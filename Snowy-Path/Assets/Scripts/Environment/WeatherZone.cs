using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeatherZone : MonoBehaviour
{
    public float radius = 10f;

    public WeatherPreset weatherPreset;

    public bool showDebug = false;

    void Start()
    {
        // Initialize the object that will contain our trigger collider
        GameObject child = new GameObject("WeatherRange");
        child.tag = "WeatherZone";
        child.transform.parent = transform;
        child.transform.localPosition = Vector3.zero;

        // Initialize the collider
        SphereCollider sphereCollider = child.AddComponent<SphereCollider>();
        sphereCollider.isTrigger = true;
        sphereCollider.radius = radius;
    }

    void OnDrawGizmos()
    {
        if (showDebug) {
            Gizmos.color = new Color(1, 0.5f, 0.5f, 0.25f);
            Gizmos.DrawSphere(transform.position, radius);
            Gizmos.DrawWireSphere(transform.position, radius);
        }
    }
}
