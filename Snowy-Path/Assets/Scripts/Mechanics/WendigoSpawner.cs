using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WendigoSpawner : MonoBehaviour {

    [SerializeField]
    private GameObject wendigo;

    [SerializeField]
    private GameObject wendigoDespawner;

    private bool wendigoIsSpawned = false;

    private Vector3 m_wendigoSpawnPosition;
    private Quaternion m_wendigoSpawnRotation;

    private void OnEnable() {
        Resetter.Reset += Reset;
    }

    private void OnDisable() {
        Resetter.Reset -= Reset;
    }

    private void Start() {
        wendigo.SetActive(false);
        m_wendigoSpawnPosition = wendigo.transform.position;
        m_wendigoSpawnRotation = wendigo.transform.rotation;
    }

    private void OnTriggerEnter(Collider other) {
        if (other.CompareTag("Player") && !wendigoIsSpawned) {
            wendigo.SetActive(true);
            wendigoIsSpawned = true;
        }
    }

    public void Reset() {
        wendigo.SetActive(false);
        wendigo.GetComponent<WendigoController>().Reset();
        wendigo.transform.position = m_wendigoSpawnPosition;
        wendigo.transform.rotation = m_wendigoSpawnRotation;
        wendigoIsSpawned = false;

        wendigoDespawner.SetActive(true);

        if (MusicManager.Instance) {
            MusicManager.Instance.ChangeParametter("WendigoEnd", 1f);
        }
    }
}
