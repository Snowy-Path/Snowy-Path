using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class HQPauseMenu : MonoBehaviour
{
    public static bool gameIsPaused;
    public GameObject OptionsMenu, GalleryMenu, CreditsMenu, PauseMenu;
    public GameObject OptionsButton, GalleryButton, CreditsButton;
    public GameObject PauseMenuFirstButton, OptionsFirstButton, GalleryFirstButton, CreditsFirstButton;



    public void Start()
    {
        //gameIsPaused = true;
        //CameraLayerToggle();
        ShowDefaultView();
        SetFocus(PauseMenuFirstButton);
        
    }
    private void OnEnable()
    {
        ShowDefaultView();
    }


    public void MainMenu()
    {
        gameIsPaused = false;
        SceneManager.LoadScene("Menu");

    }

    public void OpenOptions()
    {
        OptionsMenu.SetActive(true);
        PauseMenu.SetActive(false);
        SetFocus(OptionsFirstButton);
    }

    public void OpenCredits()
    {
        CreditsMenu.SetActive(true);
        PauseMenu.SetActive(false);
        SetFocus(CreditsFirstButton);
    }

    public void OpenGallery()
    {
        GalleryMenu.SetActive(true);
        PauseMenu.SetActive(false);
        SetFocus(GalleryFirstButton);
    }


    public void ExitOptions()
    {
        OptionsMenu.SetActive(false);
        PauseMenu.SetActive(true);
        SetFocus(OptionsButton);
    }

    public void ExitCredits()
    {
        CreditsMenu.SetActive(false);
        PauseMenu.SetActive(true);
        SetFocus(CreditsButton);
    }

    public void ExitGallery()
    {
        GalleryMenu.SetActive(false);
        PauseMenu.SetActive(true);
        SetFocus(GalleryButton);
    }

    private void SetFocus(GameObject go)
    {
        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(go);
    }
    private void ShowDefaultView()
    {
        CreditsMenu.SetActive(false);
        OptionsMenu.SetActive(false);
        GalleryMenu.SetActive(false);
        PauseMenu.SetActive(true);
    }


    public void ResumeGame()
    {
        
        this.gameObject.SetActive(false);
        Time.timeScale = 1f;
        ShowDefaultView();
        PauseMenu.SetActive(false);
        CameraLayerToggle();
        gameIsPaused = false;

    }

    public void PauseGame()
    {
        
        this.gameObject.SetActive(true);
        Time.timeScale = 0f;
        CameraLayerToggle();
        SetFocus(PauseMenuFirstButton);
        gameIsPaused = true;

    }


    public void CameraLayerToggle()
    {
        Camera cam = GameObject.Find("Camera").GetComponent<Camera>();
        //cam.cullingMask ^= 1 << LayerMask.NameToLayer("Vision");
        cam.cullingMask ^= 1 << LayerMask.NameToLayer("PlayerBody");
    }

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
}
