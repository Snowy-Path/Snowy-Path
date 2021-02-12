using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SphereTest : MonoBehaviour
{

    #region Interaction Methods

    /// <summary>
    /// Destroy itself
    /// </summary>
    public void DestroyItself() {
        Destroy(gameObject);
    }

    /// <summary>
    /// Show the outline feedback from the shader.
    /// </summary>
    public void ShowOutlineEffect() {
        GetComponent<Renderer>().material.SetFloat("IsActive", 1);
    }

    /// <summary>
    /// Hide the outline feedback from the shader.
    /// </summary>
    public void HideOutlineEffect() {
        GetComponent<Renderer>().material.SetFloat("IsActive", 0);
    }

    #endregion

}
