using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class HQMainMenu : MonoBehaviour
{
    public GameObject OptionsMenu, GalleryMenu, CreditsMenu, MainMenu;
    public GameObject OptionsButton, GalleryButton, CreditsButton;
    public GameObject MainMenuFirstButton, OptionsFirstButton, GalleryFirstButton, CreditsFirstButton;

    public void Start()
    {
        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(MainMenuFirstButton);

    }
    public void PlayGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        
    }

    public void QuitGame()
    {
        Debug.Log("Quit");
        Application.Quit();
    }

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

    public void OpenGallery()
    {
        GalleryMenu.SetActive(true);
        MainMenu.SetActive(false);
        SetFocus(GalleryFirstButton);
    }


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

    public void ExitGallery()
    {
        GalleryMenu.SetActive(false);
        MainMenu.SetActive(true);
        SetFocus(GalleryButton);
    }

    private void SetFocus(GameObject go)
    {
        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(go);
    }

}




