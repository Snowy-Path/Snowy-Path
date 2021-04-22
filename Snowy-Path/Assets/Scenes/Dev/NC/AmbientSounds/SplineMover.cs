using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SplineMover : MonoBehaviour {

    [SerializeField]
    private Spline m_spline;

    private Transform m_followObject;

    private void Start() {
        m_followObject = FindObjectOfType<PlayerController>().transform;
    }

    private void Update() {
        this.transform.position = m_spline.WhereOnSpline(m_followObject.position);
    }

}
