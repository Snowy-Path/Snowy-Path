using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IceWallReset : MonoBehaviour
{
    private void Start()
    {
        gameObject.SetActive(true);
        Resetter.Reset += Reset;

    }

    public void Reset()
    {
        gameObject.SetActive(true);
    }
}
