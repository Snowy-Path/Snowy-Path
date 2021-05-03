using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndGamePlayerScript : MonoBehaviour
{
    public void LaunchEndGameAnimation()
    {
        gameObject.GetComponent<PlayerPlayable>()?.playableEndGame.Play();
    }
}
