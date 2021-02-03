using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkullFire : MonoBehaviour
{
    public ParticleSystem ps;
    public ParticleSystem ps1;
    public bool moduleEnabled = true;
    public AudioSource firstScream;
    public AudioSource secondScream;

    void Start()
    {
        ps = gameObject.transform.GetChild(1).GetComponent<ParticleSystem>();
        ps1 = gameObject.transform.GetChild(2).GetComponent<ParticleSystem>();

    }

    void OnTriggerEnter(Collider playerCollider)
    {
        var emission = ps.emission;
        var emission1 = ps1.emission;




        if (emission.enabled == false)
        {
            firstScream.Play();
            gameObject.transform.GetChild(0).GetComponent<Light>().enabled = true;
            gameObject.transform.GetChild(3).GetComponent<Light>().enabled = true;
            emission.enabled = moduleEnabled;
            emission1.enabled = moduleEnabled;
        }

        if (GameObject.Find("Graal").GetComponent<PickupGraal>().graalIsPickedUp)
        {
            GameObject.Find("Graal").GetComponent<PickupGraal>().graalIsPickedUp = false;
            secondScream.Play();
            GameObject phantomDocks = GameObject.Find("phantomDocks");
            MeshRenderer[] docksRenderer = phantomDocks.GetComponentsInChildren<MeshRenderer>();
            MeshCollider[] docksCollider = phantomDocks.GetComponentsInChildren<MeshCollider>();
            foreach (MeshRenderer renderer in docksRenderer)
            {
                renderer.enabled = true;
            }
            foreach (MeshCollider collider in docksCollider)
            {
                collider.enabled = true;
            }
        }




    }
}
