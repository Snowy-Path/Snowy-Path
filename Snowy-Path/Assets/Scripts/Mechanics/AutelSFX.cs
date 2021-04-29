using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutelSFX : MonoBehaviour {

    private Transform m_playerTransform;
    private Vector3 m_startPosition;

    [SerializeField]
    private float m_maxRadius = 5.0f;

    private void Start() {
        m_startPosition = transform.position;
        m_playerTransform = FindObjectOfType<PlayerController>().transform;
    }

    private void Update() {

        Vector3 toPlayer = m_playerTransform.position - m_startPosition;
        if (toPlayer.magnitude <= m_maxRadius) {
            transform.position = m_startPosition + toPlayer;
        } else {
            transform.position = m_startPosition + (toPlayer.normalized * m_maxRadius);
        }
   
    }

}
