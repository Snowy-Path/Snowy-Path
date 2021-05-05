using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class PostProcessSwitch : MonoBehaviour
{
    public VolumeProfile profile;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Volume volume = FindObjectOfType<Volume>();
            if(volume != null)
            {
                volume.profile = profile;
            }
        }
    }
}
