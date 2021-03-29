using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class WendigoFollow : MonoBehaviour {

    public Transform player;
    public float speed;
    bool startFollowing;

    void Start() {
        startFollowing = false;
    }

    void Update() {

        Vector3 dir = player.position - transform.position;
        transform.rotation = Quaternion.LookRotation(new Vector3(dir.x, 0, dir.z));

        if (startFollowing && dir.magnitude > 3) {
            transform.position += dir.normalized * speed * Time.deltaTime;
        }

        if (Keyboard.current.tabKey.wasPressedThisFrame)
            startFollowing = !startFollowing;
    }
}
