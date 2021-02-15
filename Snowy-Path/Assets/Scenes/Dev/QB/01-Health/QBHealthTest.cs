using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class QBHealthTest : MonoBehaviour
{
    public GameObject player;

    void Update()
    {
        var keyboard = Keyboard.current;
        if (keyboard == null) {
            Debug.LogError("No keyboard connected");
            return;
        }

        if (keyboard.kKey.wasPressedThisFrame)
            player.GetComponent<GenericHealth>().Hit(5);
        if (keyboard.jKey.wasPressedThisFrame)
            player.GetComponent<GenericHealth>().Heal(5);
    }

    public void PrintHit(string tag, int hit)
    {
        Debug.Log("Entity with tag '" + tag + "' took " + hit + " damage");
    }

    public void PrintHitNoInfo()
    {
        Debug.Log("An entity took damage");
    }

    public void PrintHeal(string tag, int heal)
    {
        Debug.Log("Entity with tag '" + tag + "' recovered " + heal + " HP");
    }

    public void PrintHealNoInfo()
    {
        Debug.Log("An entity recovered its HP");
    }

    public void PrintDeath(string tag)
    {
        Debug.Log("Entity with tag '" + tag + "' died");
    }

    public void PrintDeathNoInfo()
    {
        Debug.Log("An entity died");
    }
}
