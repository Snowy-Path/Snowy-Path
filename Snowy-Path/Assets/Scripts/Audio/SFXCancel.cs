using FMODUnity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SFXCancel : MonoBehaviour {

    [SerializeField] StudioEventEmitter[] cancelSources;

    StudioEventEmitter emitter;

    // Start is called before the first frame update
    void Start() {
        emitter = GetComponent<StudioEventEmitter>();
    }

    // Update is called once per frame
    void Update() {
        bool cancel = false;

        foreach (var item in cancelSources) {
            if (item.IsPlaying()) {
                cancel = true;
                break;
            }
        }

        if (emitter.IsPlaying()) {

            if (cancel) emitter.Stop();
            emitter.EventInstance.setPaused(Time.timeScale == 0);
        }
    }
}
