using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FirecampLight : MonoBehaviour
{
    Light light;
    float timer;
    bool shouldGrow;

    void Start()
    {
        light = GetComponent<Light>();
        timer = Random.Range(.2f, .6f);
        shouldGrow = Random.value > .5;
    }

    void Update()
    {
        timer -= Time.deltaTime;
        if(timer <= 0)
        {
            timer = Random.Range(.2f, .6f);
            shouldGrow = !shouldGrow;
        }

        if(shouldGrow)
        {
            light.range += Random.Range(-.1f * Time.deltaTime, .5f * Time.deltaTime) * 5;
            light.intensity += Random.Range(-.1f * Time.deltaTime, .5f * Time.deltaTime) * 5;
        }
        else
        {
            light.range -= Random.Range(-.1f * Time.deltaTime, .5f * Time.deltaTime) * 5;
            light.intensity -= Random.Range(-.1f * Time.deltaTime, .5f * Time.deltaTime) * 5;
        }

        Mathf.Clamp(light.range, 2, 12);
        Mathf.Clamp(light.intensity, .8f, 1.8f);
    }
}



/*
 * using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FirecampLight : MonoBehaviour
{
    Light light;
    public float minRange = 1, maxRange = 10;
    public float minIntensity = .5f, maxIntensity = 3;
    public float speed = 50f;
    bool shouldGrow;
    bool shouldGlow;

    float targetRange;
    float targetIntensity;

    void Start()
    {
        light = GetComponent<Light>();
        shouldGrow = true;
        targetRange = maxRange + Random.Range(-3, 3);
        targetIntensity = maxIntensity + Random.Range(-.2f, .8f);
    }

    void Update()
    {
        if(shouldGrow)
        {
            if (light.range < targetRange)
            {
                light.range += Random.Range(-.1f, .3f) * Time.deltaTime * speed;
            }
            else
            {
                shouldGrow = false;
                targetRange = minRange + Random.Range(-3, 3);
            }
        }
        else
        {
            if (light.range > targetRange)
            {
                light.range -= Random.Range(-.1f, .3f) * Time.deltaTime * speed;
            }
            else
            {
                shouldGrow = true;
                targetRange = minRange + Random.Range(-3, 3);
            }
        }

        if(shouldGlow)
        {
            if (light.intensity < targetIntensity)
            {
                light.intensity += Random.Range(-.01f, .03f) * Time.deltaTime * speed;
            }
            else
            {
                shouldGlow = false;
                targetIntensity = minIntensity + Random.Range(-.2f, .8f);
            }
        }
        else
        {
            if (light.intensity > targetIntensity)
            {
                light.intensity -= Random.Range(-.01f, .03f) * Time.deltaTime * speed;
            }
            else
            {
                shouldGlow = true;
                targetIntensity = maxIntensity + Random.Range(-.2f, .8f);
            }
        }
    }
}
*/