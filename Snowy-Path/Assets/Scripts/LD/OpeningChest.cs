using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpeningChest : MonoBehaviour
{
    public string loot = "fur coat";
    bool hasBeenOpened = false;
    void OnTriggerEnter(Collider playerCollider)
    {
        
        if (hasBeenOpened == false)
        {
            GetComponent<AudioSource>().Play();
            print("You picked up a " + loot + ".");
            GetComponent<Transform>().Rotate(Vector3.up * 60f);
            
            GetComponent<Transform>().Rotate(Vector3.up * 65f);
            
            transform.position += new Vector3(0.6f,-0.4f,0f);
            
            
            hasBeenOpened = true;
        }
    }
}
