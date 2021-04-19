using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoToMenu : MonoBehaviour
{
    public void LoadMenu()
    {
        SceneLoader.Instance.LoadMainMenu();
    }

}
