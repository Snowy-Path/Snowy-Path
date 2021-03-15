using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Destructable : MonoBehaviour
{
    private void OnTriggerEnter(Collider other) {
        if (other.GetComponent<Torch>()) {
            Destroy(this.gameObject,0.3f);
        }
    }
}
