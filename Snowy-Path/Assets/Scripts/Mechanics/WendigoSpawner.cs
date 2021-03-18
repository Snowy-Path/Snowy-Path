using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WendigoSpawner : MonoBehaviour {

    [SerializeField]
    private GameObject wendigo;

    private void Start() {
        wendigo.SetActive(false);
    }

    private void OnTriggerEnter(Collider other) {
        if (other.CompareTag("Player")) {
            wendigo.SetActive(true);
            Destroy(this.gameObject);
        }
    }

}
