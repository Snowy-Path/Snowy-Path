using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class waterCanteenDrug : MonoBehaviour
{
    public bool hasBeenDrunken = false;
    public bool isSeekingGraveyard = false;

    void OnTriggerEnter(Collider playerCollider)
    {
        
        if (hasBeenDrunken == false)
        {
            GameObject.Find("blueFire").transform.GetChild(0).GetComponent<Light>().enabled = true;
            GetComponent<AudioSource>().Play();
            print("Everything seems different.");

            GetComponent<MeshRenderer>().enabled = false;
            hasBeenDrunken = true;
            GameObject.Find("Camera").GetComponent<Camera>().clearFlags = CameraClearFlags.Depth;
            isSeekingGraveyard = true;


        }
    }
}
