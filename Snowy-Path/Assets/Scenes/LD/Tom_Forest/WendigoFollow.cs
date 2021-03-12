using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class WendigoFollow : MonoBehaviour
{
    public Transform player;
    public float speed;
    bool startFollowing;

    void Start()
    {
        startFollowing = false;
    }

    void Update()
    {
        transform.rotation = Quaternion.LookRotation(player.position - transform.position);
        if(startFollowing)
        {
            transform.Translate(Vector3.forward * speed * Time.deltaTime);
        }

        if (Keyboard.current.tabKey.wasPressedThisFrame) startFollowing = true;
    }
}
