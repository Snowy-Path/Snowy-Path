using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class EndGameLoader : MonoBehaviour
{

    private void Awake()
    {
        FMOD.Studio.Bus Master = FMODUnity.RuntimeManager.GetBus("bus:/");
        Master.stopAllEvents(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
    }

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }
    public void LoadEndGame()
    {
        SceneLoader.Instance.LoadEndScene();
    }

    public void OnLoadEndGameMainMenu(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Performed)
        {
            gameObject.GetComponent<FMODUnity.StudioEventEmitter>()?.Stop();
            SceneLoader.Instance.LoadMainMenu();
        }
    }

    public void LoadEndGameMainMenu()
    {
        gameObject.GetComponent<FMODUnity.StudioEventEmitter>()?.Stop();
        SceneLoader.Instance.LoadMainMenu();
    }
}
