using FMODUnity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColdBreathAudio : MonoBehaviour {
    [Header("FMOD events")]
    [EventRef] [SerializeField] string lightBreath;
    [EventRef] [SerializeField] string mediumBreath;
    [EventRef] [SerializeField] string heavyBreath;

    [Header("Thresholds")]
    [SerializeField] float lightThresPercent;
    [SerializeField] float mediumThresPercent;
    [SerializeField] float heavyThresPercent;

    [Header("Random timer")]
    [SerializeField] float minDelay;
    [SerializeField] float maxDelay;

    private Temperature temperature;
    private StudioEventEmitter emitter;

    private float timer = 0;
    private float delay = 0;

    // Start is called before the first frame update
    void Start() {
        temperature = GetComponentInParent<Temperature>();
        emitter = GetComponent<StudioEventEmitter>();
    }

    // Update is called once per frame
    void Update() {
        float tempRatio = Mathf.Clamp(temperature.CurrentTemperature / temperature.maxTemperature, 0, 1);
        bool play = true;
        if (tempRatio < heavyThresPercent) {
            emitter.Event = lightBreath;
        }
        else if (tempRatio < mediumThresPercent) {
            emitter.Event = mediumBreath;
        }
        else if (tempRatio < lightThresPercent) {
            emitter.Event = lightBreath;
        }
        else
            play = false;

        if (play) {
            timer += Time.deltaTime;

            if (timer >= delay) {
                emitter.Play();
                timer = 0;
                delay = Random.Range(minDelay, maxDelay);
            }

        }
    }
}
