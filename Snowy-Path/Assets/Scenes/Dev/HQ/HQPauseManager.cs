using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class HQPauseManager : MonoBehaviour
{

    public GameObject pauseMenuprefab;
    private HQPauseMenu pauseMenu;

    private void Start()
    {

    }

    void Update()
    {
        if (Keyboard.current.pKey.wasPressedThisFrame)
        {
            Pause();
        }
    }

    /// <summary>
    /// Instantiate the Pause Menu prefab in front of player and toggle pause when pressed
    /// </summary>
    public void Pause()
    {
        if (pauseMenu == null)
        {
            pauseMenu = Instantiate(pauseMenuprefab, transform.position, Quaternion.identity).GetComponent<HQPauseMenu>();
            pauseMenu.PauseGame();
        }
        else
        {
            pauseMenu.Toggle();

        }

        pauseMenu.transform.position = transform.position;
        pauseMenu.transform.rotation = transform.rotation;

    }
}