using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapPin : MonoBehaviour
{
    public float scale = 1f;

    public int pinType = -1;

    public void UpdateScale()
    {
        transform.localScale = new Vector3(
            scale / transform.parent.localScale.x,
            scale / transform.parent.localScale.y,
            scale / transform.parent.localScale.z
        );
    }
}
