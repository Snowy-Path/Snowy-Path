using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IllusionMaskTrigger : MonoBehaviour {

    [SerializeField]
    private GameObject wendigoIllusionMask;
    
    [SerializeField]
    private GameObject rocksIllusionMask;

    private void OnTriggerEnter(Collider other) {
        if (other.CompareTag("Player")) {
            wendigoIllusionMask.SetActive(true);
            rocksIllusionMask.SetActive(true);
        }
    }

    private void OnTriggerExit(Collider other) {
        if (other.CompareTag("Player")) {
            wendigoIllusionMask.SetActive(false);
            rocksIllusionMask.SetActive(false);
        }
    }

}
