using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Light))]
public class FlickeringLight : MonoBehaviour {

    private Light m_flickeringLight;

    [SerializeField]
    private float intensityMin = 0.5f;

    [SerializeField]
    private float intensityMax = 1f;

    private float m_previousIntensity;
    private float m_nextIntensity;

    [SerializeField]
    private float timerCooldown = 0.3f;

    private float timer = 0f;

    private void Start() {
        m_flickeringLight = GetComponent<Light>();
        m_previousIntensity = m_flickeringLight.intensity;
    }

    // Update is called once per frame
    void Update() {

        timer += Time.deltaTime;
        if (timer >= timerCooldown) {
            m_previousIntensity = m_flickeringLight.intensity;
            m_nextIntensity = Random.Range(intensityMin, intensityMax);
            timer = 0f;
        }

        m_flickeringLight.intensity = Mathf.Lerp(m_previousIntensity, m_nextIntensity, timer / timerCooldown);

    }
}
