using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelfDestroy : MonoBehaviour
{
    [Min(0)]
    [SerializeField] float lifeTime = 1f;

    private float timer = 0f;

    void Update()
    {
        timer += Time.deltaTime;
        if (timer > lifeTime) {
            Destroy(this.gameObject);
        }
    }
}
