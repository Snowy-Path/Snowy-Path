using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class lightingFirecamp : MonoBehaviour
{
    
        private ParticleSystem ps;
        public bool moduleEnabled = true;
    

    void Start()
        {
            ps = gameObject.transform.GetChild(1).GetComponent<ParticleSystem>();
        
    }

  
        void OnTriggerEnter(Collider playerCollider)
    {
        var emission = ps.emission;
        if (emission.enabled == false)
        {
            GetComponent<AudioSource>().Play();
            print("Firecamp lightened.");

            gameObject.transform.GetChild(0).GetComponent<Light>().enabled = true;
            emission.enabled = moduleEnabled;
        }

  

        
            

        
    }
}
