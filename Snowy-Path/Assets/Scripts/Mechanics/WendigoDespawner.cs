using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WendigoDespawner : MonoBehaviour {

    [SerializeField]
    private GameObject wendigo;

    [SerializeField]
    private GameObject wendigoSpawner;

    private void OnTriggerEnter(Collider other) {
        if (other.CompareTag("Player")) {
            wendigo.GetComponent<WendigoController>().PlayDisappearingAnimation();
            gameObject.SetActive(false);
            wendigoSpawner.SetActive(false);
        }
    }

}
