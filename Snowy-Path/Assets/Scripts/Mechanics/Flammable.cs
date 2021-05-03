using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Flammable : MonoBehaviour {

    #region Variables
    [SerializeField] GameObject fireFX;
    [SerializeField] Transform ignitePointsParent;
    [SerializeField] float fireDuration;
    [SerializeField] float maxDissolve = .4f;

    private bool ignited = false;
    private float dissolve = 0f;
    private float dissolveStep;
    private const float timeStep = .1f;

    private List<Transform> fireParticles;
    private List<GameObject> GFXs;

    [SerializeField]
    private List<GameObject> m_additionalGOToDeactivate;
    #endregion

    /// <summary>
    /// Called when SetActive(true).
    /// Subscribe to the Resetter.
    /// </summary>
    private void OnEnable() {
        Resetter.Reset += Reset;
    }

    /// <summary>
    /// Called when SetActive(false).
    /// Unsubscribe to the Resetter.
    /// </summary>
    private void OnDisable() {
        Resetter.Reset -= Reset;
    }

    /// <summary>
    /// Reset the gameobject.
    /// </summary>
    public void Reset() {
        foreach (var gfx in GFXs) {
            gfx.SetActive(true);
        }
        foreach (var go in m_additionalGOToDeactivate) {
            go.SetActive(true);
        }
        ResetDissolve();
        ignited = false;
    }

    /// <summary>
    /// Retrieves GFX & instantiate fire particles.
    /// </summary>
    void Start() {

        //Retrives the GFXs children
        MeshFilter[] meshFilters = GetComponentsInChildren<MeshFilter>();
        GFXs = new List<GameObject>(meshFilters.Length);
        foreach (var mesh in meshFilters) {
            GFXs.Add(mesh.gameObject);
        }

        //Instantiate a particle system for each ingnite point.
        List<Transform> ignitePoints = ignitePointsParent.GetComponentsInChildren<Transform>().ToList();
        ignitePoints.Remove(this.transform);

        fireParticles = new List<Transform>(ignitePoints.Count);

        for (int i = 0; i < ignitePoints.Count; i++) {
            GameObject go = Instantiate(fireFX, ignitePoints[i].position, Quaternion.identity, transform);
            fireParticles.Add(go.transform);
        }

        //Init dissolve step
        dissolveStep = timeStep * maxDissolve / fireDuration;
    }

    /// <summary>
    /// Start the coroutine to destroy the brambles.
    /// </summary>
    /// <param name="other"></param>
    private void OnTriggerEnter(Collider other) {
        if (!ignited && other.GetComponent<Torch>()) {
            ignited = true;
            fireParticles = ReorderPoints(other.transform.position);
            StartCoroutine(StartFire());
            ApplyDissolve();
        }
    }

    /// <summary>
    /// Coroutines that start SFX and Particles of fire.
    /// Deactivate GFX at the end.
    /// </summary>
    /// <returns></returns>
    IEnumerator StartFire() {

        for (int i = 0; i < fireParticles.Count; i++) {
            fireParticles[i].GetComponent<ParticleSystem>().Play();
            fireParticles[i].GetComponent<FMODUnity.StudioEventEmitter>().Play();
            yield return new WaitForSeconds(fireDuration / fireParticles.Count);
        }

        for (int i = 0; i < fireParticles.Count; i++) {
            fireParticles[i].GetComponent<ParticleSystem>().Stop();
        }

        foreach (var gfx in GFXs) {
            gfx.SetActive(false);
        }

        foreach(var go in m_additionalGOToDeactivate) {
            go.SetActive(false);
        }

        for (int i = 0; i < fireParticles.Count; i++) {
            fireParticles[i].GetComponent<FMODUnity.StudioEventEmitter>().Stop();
            yield return new WaitForSeconds(.05f);
        }
    }

    /// <summary>
    /// Order ignites points depending on the hit position.
    /// </summary>
    /// <param name="start"></param>
    /// <returns></returns>
    private List<Transform> ReorderPoints(Vector3 start) {
        return fireParticles.OrderBy(o => (o.position - start).magnitude).ToList();
    }

    private void ApplyDissolve() {

        dissolve += dissolveStep;
        foreach (var mesh in GFXs) {
            MeshRenderer renderer = mesh.GetComponent<MeshRenderer>();
            renderer.material.SetFloat("_Progress", dissolve);
        }

        if (dissolve < maxDissolve) {
            Invoke(nameof(ApplyDissolve), timeStep);
        }
    }

    private void ResetDissolve() {
        foreach (var mesh in GFXs) {
            MeshRenderer renderer = mesh.GetComponent<MeshRenderer>();
            renderer.material.SetFloat("_Progress", 0);
        }
        dissolve = 0;
        dissolveStep = timeStep * maxDissolve / fireDuration;
    }
}
