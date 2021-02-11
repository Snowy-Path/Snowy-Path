using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Simple interaction test script.
/// Derives from InteractionBase.
/// Change the light color to blue.
/// </summary>
public class InteractionTestScript : InteractionBase {

    #region Variables

    public Light cubeLight;
    private bool _isBlue = false;

    private Color _baseColor;

    #endregion

    #region Built In Methods

    /// <summary>
    /// Called at start. Store the base color.
    /// </summary>
    void Start() {
        _baseColor = cubeLight.color;
    }

    #endregion

    #region Override Methods

    public override void Interact() {

        if (!_isBlue) {
            cubeLight.color = Color.blue;
        } else {
            cubeLight.color = _baseColor;
        }
        _isBlue = !_isBlue;
    }

    public override void ShowInteractionFeedback() {
        Debug.Log("SHOW");
        GetComponent<Renderer>().material.SetFloat("Boolean_917F005B", 1);
    }

    public override void HideInteractionFeedback() {
        Debug.Log("HIDE");
        GetComponent<Renderer>().material.SetFloat("Boolean_917F005B", 0);
    }

    #endregion

}
