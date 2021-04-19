using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class MeshFadeOut : MonoBehaviour {
    public float cooldown = 1f;
    public float fadeOutRate = 0.8f;

    private MeshRenderer renderer;
    private Material mat;
    private float timer;

    void Start() {
        renderer = gameObject.GetComponent<MeshRenderer>();
        mat = renderer.material;
        timer = cooldown;
    }

    void Update() {

        if (timer > 0) {
            timer -= Time.deltaTime;
        }
        else{
            if (mat.color.a > 0) {
                Color newColor = mat.color;
                newColor.a -= fadeOutRate * Time.deltaTime;
                mat.color = newColor;
                renderer.material = mat;
            }
            else
                Destroy(gameObject);
        }
        
    }
}
