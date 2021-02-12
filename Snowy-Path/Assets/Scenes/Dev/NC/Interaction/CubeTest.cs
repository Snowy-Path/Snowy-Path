using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Simple interaction test script.
/// Change the directional light color to blue.
/// </summary>
public class CubeTest : MonoBehaviour {

    #region Variables

    public Light cubeLight;
    private bool _isBlue = false;

    private Color _baseColor;

    #endregion

    #region Built In Methods

    /// <summary>
    /// Called at start.
    /// Store the base color.
    /// </summary>
    void Start() {
        _baseColor = cubeLight.color;
    }

    #endregion

    #region Interaction Methods

    /// <summary>
    /// Switch the color to blue.
    /// And then back to the normal.
    /// </summary>
    public void SwitchColor() {

        if (!_isBlue) {
            cubeLight.color = Color.blue;
        } else {
            cubeLight.color = _baseColor;
        }
        _isBlue = !_isBlue;
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
