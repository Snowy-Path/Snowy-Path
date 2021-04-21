using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class SpawnScript : MonoBehaviour
{
    public PlayableDirector wakeUp;

    private void Start()
    {
        wakeUp.stopped += SpawnCharacterReset;
    }
    public void Spawn()
    {
            GetComponent<PlayerCampfireSave>().RestorePlayerAtCampfire();
            wakeUp.Play();
    }

    private void SpawnCharacterReset(PlayableDirector action)
    {
        if(action == wakeUp)
        {
            CharacterController charController = GetComponent<CharacterController>();
            PlayerController playerController = GetComponent<PlayerController>();
            charController.enabled = false;
            playerController.enabled = false;
            this.transform.eulerAngles = new Vector3(0, 0, 0);
            charController.enabled = true;
            playerController.enabled = true;
        }
    }

}
