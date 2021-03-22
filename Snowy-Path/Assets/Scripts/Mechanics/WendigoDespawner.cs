using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WendigoDespawner : MonoBehaviour {

    [SerializeField]
    private GameObject wendigo;

    private void OnTriggerEnter(Collider other) {
        if (other.CompareTag("Player")) {
            wendigo.SetActive(false);
            Destroy(this.gameObject);
        }
    }

}
