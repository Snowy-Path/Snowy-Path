using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CapsuleTest : MonoBehaviour
{
    #region Interaction Methods

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
