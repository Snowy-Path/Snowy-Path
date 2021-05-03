using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMODUnity;

public class SFXSprintRecovery : MonoBehaviour {

    private PlayerController controller;
    private StudioEventEmitter audioEmitter;
    private bool inRecovery = false;
    private bool isPlaying = false;

    // Start is called before the first frame update
    void Start() {
        controller = GetComponentInParent<PlayerController>();
        audioEmitter = GetComponent<StudioEventEmitter>();
    }

    // Update is called once per frame
    void Update() {

        inRecovery = !controller.IsRunning && controller.SprintTimer > 0;

        if (inRecovery) {
            if (!isPlaying) {
                audioEmitter.Play();
                isPlaying = true;
            }
            audioEmitter.EventInstance.setVolume(Mathf.Clamp(controller.SprintTimer, 0f, 1f));
        }
        else {
            audioEmitter.Stop();
            isPlaying = false;
        }

    }
}
