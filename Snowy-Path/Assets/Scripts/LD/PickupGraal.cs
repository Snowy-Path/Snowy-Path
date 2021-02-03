using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickupGraal : MonoBehaviour
{
    public ParticleSystem ps;
    public ParticleSystem ps1;
    public bool moduleDisabled = false;
    public bool graalIsPickedUp = false;


    void Start()
    {
        ps = GameObject.Find("blueFire").GetComponent<ParticleSystem>();
        

    }


    void OnTriggerEnter(Collider playerCollider)
    {
        var emission = ps.emission;
        

                
        if (GameObject.Find("blueFire").GetComponent<BlueFireScript>().graalCanBePickedUp == true)
        {
            GameObject.Find("blueFire").transform.GetChild(0).GetComponent<Light>().enabled = false;
            GameObject.Find("blueFire").GetComponent<BlueFireScript>().graalCanBePickedUp = false;
            GetComponent<AudioSource>().Play();
            print("This is a cup.");
            emission.enabled = moduleDisabled;
            
            graalIsPickedUp = true;
            GetComponent<MeshRenderer>().enabled = false;
            


        }
    }
}
