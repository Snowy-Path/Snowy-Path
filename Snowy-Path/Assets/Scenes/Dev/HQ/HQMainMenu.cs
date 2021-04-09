using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class HQMainMenu : MonoBehaviour
{
    public GameObject OptionsMenu, CreditsMenu, MainMenu;
    public GameObject OptionsButton, CreditsButton;
    public GameObject MainMenuFirstButton, OptionsFirstButton, CreditsFirstButton;
    public PlayerInput playerInput;
    public void Start()
    {

        ////playerInput = FindObjectOfType<PlayerInput>();
        //playerInput.SwitchCurrentActionMap("PauseMenu");
        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(MainMenuFirstButton);


    }
    public void PlayGame()
    {
        //playerInput.SwitchCurrentActionMap("Gameplay");
        SceneLoader.Instance.LoadWorld();
        SaveSystem.Instance.SetCurrentSave(0);

    }

    public void QuitGame()
    {
        Debug.Log("Quit");
        Application.Quit();
    }


    #region OPEN MENUS
        //Set active the opened menu and desactivate the previous one
    public void OpenOptions()
    {
        OptionsMenu.SetActive(true);
        MainMenu.SetActive(false);
        SetFocus(OptionsFirstButton);
    }

    public void OpenCredits()
    {
        CreditsMenu.SetActive(true);
        MainMenu.SetActive(false);
        SetFocus(CreditsFirstButton);
    }


#endregion

    #region CLOSE MENUS
    //Set unactive the closed menu and reactivate the pause menu
    public void ExitOptions()
    {
        OptionsMenu.SetActive(false);
        MainMenu.SetActive(true);
        SetFocus(OptionsButton);
    }

    public void ExitCredits()
    {
        CreditsMenu.SetActive(false);
        MainMenu.SetActive(true);
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
        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(go);
    }

    /// <summary>
    /// Default MainMenu view
    /// </summary>
    private void ShowDefaultView()
    {
        CreditsMenu.SetActive(false);
        OptionsMenu.SetActive(false);
        MainMenu.SetActive(true);
    }
    #endregion
}




