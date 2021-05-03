using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndGameLoader : MonoBehaviour
{
    public void LoadEndGame()
    {
        SceneLoader.Instance.LoadEndScene();
    }

    public void LoadEndGameMainMenu()
    {
        SceneLoader.Instance.LoadMainMenu();
    }
}
