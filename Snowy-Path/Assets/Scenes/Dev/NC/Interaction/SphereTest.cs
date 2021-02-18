using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SphereTest : MonoBehaviour
{

    #region Interaction Methods

    /// <summary>
    /// Show the outline feedback from the shader.
    /// </summary>
    public void ShowOutlineEffect() {
        GetComponent<Renderer>().materials[0].SetFloat("IsActive", 1);
    }

    /// <summary>
    /// Hide the outline feedback from the shader.
    /// </summary>
    public void HideOutlineEffect() {
        //GetComponent<Renderer>().material.SetFloat("IsActive", 0);
        GetComponent<Renderer>().materials[0].SetFloat("IsActive", 0);
    }

    #endregion

    private void OnDestroy() {
        for (int i = 0; i < GetComponent<Renderer>().materials.Length; i++) {
            Destroy(GetComponent<Renderer>().materials[i]);
        }
    }

}
