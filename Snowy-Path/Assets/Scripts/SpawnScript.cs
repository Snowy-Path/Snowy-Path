using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class SpawnScript : MonoBehaviour
{
    public PlayableDirector wakeUp;
    public void Spawn()
    {
        // If the last campfire ID is nothing then it mean the player never played and will wake up at the spawn point
        if (GetComponent<PlayerCampfireSave>().LastCampfireId == "")
        {
            CharacterController charController = GetComponent<CharacterController>();
            charController.enabled = false;
            this.transform.position = FindObjectOfType<SpawnPlayerPosition>().transform.position;
            this.transform.eulerAngles = new Vector3(14, -104, -83);
            charController.enabled = true;
            wakeUp.Play();

        }
        else
        {
            GetComponent<PlayerCampfireSave>().RestorePlayerAtCampfire();
            wakeUp.Play();

        }
    }

}
