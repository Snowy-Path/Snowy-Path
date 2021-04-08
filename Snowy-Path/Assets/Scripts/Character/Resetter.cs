using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Resetter : MonoBehaviour {

    public delegate void ResetEvent();
    public static event ResetEvent Reset;

    public void ResetAll() {
        if (Reset != null) {
            Reset();
        }
    }

}
