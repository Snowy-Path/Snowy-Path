using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WendigoMusicTrigger : MonoBehaviour {

    [SerializeField]
    private string m_parametterName;

    [SerializeField]
    private float m_parametterValue;

    private void OnTriggerEnter(Collider other) {
        if (other.CompareTag("Player")) {
            if (MusicManager.Instance) {
                MusicManager.Instance.ChangeParametter(m_parametterName, m_parametterValue);
            }
        }
    }
}
