using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class QBHeat_PlayerController : MonoBehaviour
{
    public float speed = 1f;

    public Cloth clothAsset;

    void Start()
    {
        GetComponent<Inventory>().ChangeCloth(Object.Instantiate(clothAsset));
    }

    void Update()
    {
        Vector3 velocity = Vector3.zero;

        var keyboard = Keyboard.current;
        if (keyboard.leftArrowKey.isPressed)
            velocity.x = -1;
        else if (keyboard.rightArrowKey.isPressed)
            velocity.x = 1;
        if (keyboard.upArrowKey.isPressed)
            velocity.z = 1;
        else if (keyboard.downArrowKey.isPressed)
            velocity.z = -1;

        transform.position = transform.position + velocity * speed * Time.deltaTime;
    }
}
