using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndGameLoader : MonoBehaviour
{
    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }
    public void LoadEndGame()
    {
        SceneLoader.Instance.LoadEndScene();
    }

    public void LoadEndGameMainMenu()
    {
        gameObject.GetComponent<FMODUnity.StudioEventEmitter>()?.Stop();
        SceneLoader.Instance.LoadMainMenu();
    }
}
