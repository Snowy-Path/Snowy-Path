using FMODUnity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SFXColdBreath : MonoBehaviour {

    [Header("FMOD emitters")]
    [SerializeField] StudioEventEmitter lightBreath;
    [SerializeField] StudioEventEmitter mediumBreath;
    [SerializeField] StudioEventEmitter heavyBreath;
    [SerializeField] float breathLength;

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
    private bool isPlaying = false;

    // Start is called before the first frame update
    void Start() {
        temperature = GetComponentInParent<Temperature>();
        emitter = GetComponent<StudioEventEmitter>();
    }

    // Update is called once per frame
    void Update() {
        float tempRatio = Mathf.Clamp(temperature.CurrentTemperature / temperature.maxTemperature, 0, 1);
        bool breathTrigger = true;
        if (tempRatio < heavyThresPercent) {
            emitter = heavyBreath;
        }
        else if (tempRatio < mediumThresPercent) {
            emitter = mediumBreath;
        }
        else if (tempRatio < lightThresPercent) {
            emitter = lightBreath;
        }
        else
            breathTrigger = false;
       
        if (breathTrigger && !isPlaying) {
            timer += Time.deltaTime;

            if (timer >= delay) {
                isPlaying = true;
                emitter.EventInstance.setVolume(1f);
                emitter.Play();
                Invoke(nameof(ResetTimer), breathLength);
            }
        }
    }

    public void ResetTimer() {
        timer = 0;
        isPlaying = false;
        delay = Random.Range(minDelay, maxDelay);
        StartCoroutine(FadeOut());
    }

    private IEnumerator FadeOut() {
        float volume = 1;
        while (volume > 0) {
            emitter.EventInstance.getVolume(out volume);
            emitter.EventInstance.setVolume(Mathf.Clamp(volume - 0.01f, 0f, 1f));
            yield return null;
        }
    }
}
