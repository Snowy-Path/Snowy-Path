using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class HQPauseMenu : MonoBehaviour
{
    //private OptionSettings inputDevice;
    public static bool gameIsPaused;
    public GameObject OptionsMenu, CreditsMenu, PauseMenu, PlayMenu;
    public GameObject OptionsButton, CreditsButton;
    public GameObject PauseMenuFirstButton, OptionsFirstButton, CreditsFirstButton;
    public PlayerInput playerInput;
 

    public void Start()
    {

        ShowDefaultView();
        SetFocus(PauseMenuFirstButton);

        
    }
    private void OnEnable()
    {
        ShowDefaultView();
    }

    public void OnPause(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Performed)
        {
            PauseGame();
        }
    }
    public void OnResume(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Performed)
        {
            ResumeGame();
        }
    }

    #region OPEN MENUS
    //Set active the opened menu and desactivate the previous one

    /// <summary>
    /// Load The Menu scene
    /// </summary>
    public void MainMenu()
    {
        gameIsPaused = false;
        ResumeGame();
        SceneSave sceneSave = FindObjectOfType<SceneSave>();
        if(sceneSave != null)
        {
            sceneSave.SceneName = "";
        }
        SceneLoader.Instance.LoadMainMenu();

    }
    
    public void OpenOptions()
    {
        OptionsMenu.SetActive(true);
        PauseMenu.SetActive(false);
        SetFocus(OptionsFirstButton);
    }

    public void OpenCredits()
    {
        //CreditsMenu.SetActive(true);
        PauseMenu.SetActive(false);
        SetFocus(CreditsFirstButton);
    }

    #endregion

    #region CLOSE MENUS
    //Set unactive the closed menu and reactivate the pause menu
    public void ExitOptions()
    {
        OptionsMenu.SetActive(false);
        PauseMenu.SetActive(true);
        SetFocus(OptionsButton);
    }

    public void ExitCredits()
    {
        //CreditsMenu.SetActive(false);
        PauseMenu.SetActive(true);
        SetFocus(CreditsButton);
    }

    #endregion

    #region NAVIGATION METHODS

    /// <summary>
    /// Set the selsection focus on the button go
    /// </summary>
    /// <param name="go"></param>
    private void SetFocus(GameObject go)
    {
        if (OptionHandler.gamepadconnected)
        {
            EventSystem.current.SetSelectedGameObject(null);
            EventSystem.current.SetSelectedGameObject(go);
        }

    }

    /// <summary>
    /// Default PauseMenu view
    /// </summary>
    private void ShowDefaultView()
    {
        //CreditsMenu.SetActive(false);
        OptionsMenu.SetActive(false);
        PauseMenu.SetActive(true);
        PlayMenu.SetActive(false);
    }

    /// <summary>
    /// Resume the game, timescale at 1
    /// </summary>
    public void ResumeGame()
    {
        //GetComponentInChildren<Canvas>().gameObject.SetActive(false);
        this.gameObject.SetActive(false);
        Time.timeScale = 1f;
        ShowDefaultView();
        PauseMenu.SetActive(false);
        CameraLayerToggle();
        gameIsPaused = false;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        playerInput.SwitchCurrentActionMap("Gameplay");



    }
    /// <summary>
    /// Pause the game, timescale at 0
    /// </summary>
    public void PauseGame()
    {
        //GetComponentInChildren<Canvas>().gameObject.SetActive(true);
        this.gameObject.SetActive(true);
        Time.timeScale = 0f;
        CameraLayerToggle();
        SetFocus(PauseMenuFirstButton);
        gameIsPaused = true;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        playerInput.SwitchCurrentActionMap("PauseMenu");

    }

    /// <summary>
    /// Change the culling mask to hide Player Body during Pause
    /// </summary>
    public void CameraLayerToggle()
    {
        Camera cam = GameObject.Find("Camera").GetComponent<Camera>();
        //cam.cullingMask ^= 1 << LayerMask.NameToLayer("Vision");
        cam.cullingMask ^= 1 << LayerMask.NameToLayer("PlayerBody");
    }

    /// <summary>
    /// Toggle between pause and resume game
    /// </summary>
    public void Toggle()
    {
        if (!gameIsPaused)
        {
            PauseGame();
        }

        else
        {
            ResumeGame();
        }

    }
    #endregion
}
