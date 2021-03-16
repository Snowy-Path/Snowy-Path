using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelLoader : MonoBehaviour
{
    public GameScene level;


    void ShowScene()
    {
        // get a reference to the scene you want to search. 
        Scene s = SceneManager.GetSceneByName(level.sceneName);
        if (s.IsValid())
        {
            GameObject[] gameObjects = s.GetRootGameObjects();

            foreach (GameObject item in gameObjects)
            {
                if (item.CompareTag("SceneWorld"))
                {
                    item.SetActive(true);
                }
            }
        }
    }

    void HideScene()
    {
        // get a reference to the scene you want to search. 
        Scene s = SceneManager.GetSceneByName(level.sceneName);
        if (s.IsValid())
        {
            GameObject[] gameObjects = s.GetRootGameObjects();

            foreach (GameObject item in gameObjects)
            {
                if (item.CompareTag("SceneWorld"))
                {
                    item.SetActive(false);
                }
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            ShowScene();
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            ShowScene();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            HideScene();
        }
    }
}
