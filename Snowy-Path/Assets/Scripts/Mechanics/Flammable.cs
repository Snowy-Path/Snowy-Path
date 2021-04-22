using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Flammable : MonoBehaviour {

    [SerializeField] GameObject fireFX;
    [SerializeField] Transform ignitePointsParent;
    [SerializeField] float fireDuration;
    private List<Transform> ignitePoints;

    private bool ignited = false;
    private bool hasAudio = false;

    private void OnEnable() {
        Resetter.Reset += Reset;
    }

    private void OnDisable() {
        Resetter.Reset -= Reset;
    }

    public void Reset() {
        
    }

    // Start is called before the first frame update
    void Start() {
        ignitePoints = ignitePointsParent.GetComponentsInChildren<Transform>().ToList();
        ignitePoints.Remove(this.transform);
    }

    private void OnTriggerEnter(Collider other) {
        if (!ignited && other.GetComponent<Torch>()) {
            ignited = true;
            ignitePoints = ReorderPoints(other.transform.position);
            StartCoroutine(StartFire());
            //Destroy(this.gameObject, 3f);
        }
    }

    IEnumerator StartFire() {
        ParticleSystem[] particles = new ParticleSystem[ignitePoints.Count];

        for (int i = 0; i < ignitePoints.Count; i++) {
            GameObject go = Instantiate(fireFX, ignitePoints[i].position, Quaternion.identity);
            var audio = go.GetComponent<FMODUnity.StudioEventEmitter>();
            if (audio) {
                if (!hasAudio)
                    hasAudio = true;
                else
                    audio.enabled = false;
            }

            ParticleSystem particle = go.GetComponent<ParticleSystem>();
            particles[i] = particle;
            yield return new WaitForSeconds(fireDuration / ignitePoints.Count);
        }

        for (int i = 0; i < particles.Length; i++) {
            particles[i].Stop();
        }
        Destroy(this.gameObject);
    }

    private List<Transform> ReorderPoints(Vector3 start) {
        return ignitePoints.OrderBy(o => (o.position - start).magnitude).ToList();
    }
}
