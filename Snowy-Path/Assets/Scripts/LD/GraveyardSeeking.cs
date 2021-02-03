using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GraveyardSeeking : MonoBehaviour
{
    public bool hasFoundGraveyard = false;

    void OnTriggerEnter(Collider playerCollider)
    {
        

        if (GameObject.Find("Item_WaterCanteen").GetComponent<waterCanteenDrug>().isSeekingGraveyard == true)
        {
            hasFoundGraveyard = true;
            GetComponent<AudioSource>().Play();
            GameObject.Find("Item_WaterCanteen").GetComponent<waterCanteenDrug>().isSeekingGraveyard = false;
            print("Everything seems different.");
            GameObject.Find("Camera").GetComponent<Camera>().clearFlags = CameraClearFlags.Skybox;

            
           



        }
    }
}
