using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractionTestScript : InteractionBase {

    public Light cubeLight;
    private bool _isBlue = false;

    private Color _baseColor;

    void Start() {
        _baseColor = cubeLight.color;
    }

    public override void Interact() {

        if (!_isBlue) {
            cubeLight.color = Color.blue;
        } else {
            cubeLight.color = _baseColor;
        }
        _isBlue = !_isBlue;
    }
}
