using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelLoader : MonoBehaviour
{
    public GameScene level;
    public int priority = 0;
    internal bool isSceneNeedToBeShowned = false;
    internal bool isSceneNeedToBeHided = false;

    private void Update()
    {
        if (isSceneNeedToBeShowned)
        {
            ShowScene();
        }
        if (isSceneNeedToBeHided )
        {
            HideScene();
        }
    }

    LevelLoader GetLevelLoaderHighestPriority()
    {
        LevelLoader levelLoaderHighestPriority = null;
        SceneLoader sceneLoader = FindObjectOfType<SceneLoader>();
        if (sceneLoader != null)
        {
            foreach (LevelLoader lvlLoader in sceneLoader.levelLoadersActive)
            {
                if (levelLoaderHighestPriority == null) 
                {
                    levelLoaderHighestPriority = lvlLoader;
                }
                else
                {
                    if (lvlLoader.priority > levelLoaderHighestPriority.priority)
                    {
                        levelLoaderHighestPriority = lvlLoader;
                    }
                }
            }
        }

        return levelLoaderHighestPriority;
        //Scene activeScene = SceneManager.GetActiveScene();
        //foreach (var item in activeScene.GetRootGameObjects())
        //{
        //    LevelLoader lvlLoaderActive = item.GetComponent<LevelLoader>();
        //    if (lvlLoaderActive != null)
        //    {
        //        if(lvlLoaderActive.priority <= priority)
        //        {
        //            return true;
        //        }
        //    }
        //}
        //return false;
    }

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
                    // Set the active scene to the highest priority scene
                    //SceneManager.SetActiveScene(SceneManager.GetSceneByName(GetLevelLoaderHighestPriority().level.sceneName));
                    SceneManager.SetActiveScene(s);
                    item.SetActive(true);
                    isSceneNeedToBeShowned = false;
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
                    //SceneLoader sceneLoader = FindObjectOfType<SceneLoader>();
                    //if (sceneLoader != null)
                    //{
                    //    foreach (LevelLoader lvlLoader in sceneLoader.levelLoadersActive)
                    //    {
                    //        if (lvlLoader != this)
                    //        {
                    //            SceneManager.SetActiveScene(SceneManager.GetSceneByName(lvlLoader.level.sceneName));
                    //        }
                    //    }
                    //}

                    item.SetActive(false);
                    isSceneNeedToBeHided = false;
                }
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            // Add the sceneLoader to the active sceneloader list
            SceneLoader sceneLoader = FindObjectOfType<SceneLoader>();
            if (sceneLoader != null)
            {
                sceneLoader.levelLoadersActive.Add(this);
            }

            isSceneNeedToBeShowned = true;
        }
    }

    private void OnTriggerStay(Collider other)
    {
        //if (other.CompareTag("Player"))
        //{
        //    ShowScene();
        //}
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            // Remove the sceneLoader from the active sceneloader list
            SceneLoader sceneLoader = FindObjectOfType<SceneLoader>();
            if (sceneLoader != null)
            {
                sceneLoader.levelLoadersActive.Remove(this);
            }
            isSceneNeedToBeHided = true;
        }
    }
}
