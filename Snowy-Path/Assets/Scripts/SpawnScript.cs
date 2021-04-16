using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class SpawnScript : MonoBehaviour
{
    public PlayableDirector wakeUp;
    public void Spawn()
    {
            GetComponent<PlayerCampfireSave>().RestorePlayerAtCampfire();
            wakeUp.Play();
    }

}
