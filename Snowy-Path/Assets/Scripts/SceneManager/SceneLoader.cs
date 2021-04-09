using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.Timeline;
using UnityEngine.UI;

public class SceneLoader : MonoBehaviour
{
    public ScenesData sceneDataBase;
    public GameObject loadingScreen;
    public Slider slider;
    public Text loadingProgressText;
    public static SceneLoader Instance;
    private List<AsyncOperation> scenesToLoad = new List<AsyncOperation>();
    bool worldHasLoaded = false;
    bool isPlayerLoaded = false;
    bool isLoadingCompleted = false;


    private void Awake()
    {
        // Singleton of the GameManager
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(Instance);
            Instance = gameObject.GetComponent<SceneLoader>();
            return;
        }
        DontDestroyOnLoad(this.gameObject);
        SceneManager.sceneLoaded += OnSceneLoaded;

    }

    private void Start()
    {
        // We load the main menu
        LoadMainMenu();
    }

    private void LateUpdate()
    {
        if (isPlayerLoaded && !isLoadingCompleted)
            Spawn();

        if (worldHasLoaded && !isPlayerLoaded)
        {
            if (!SceneManager.GetSceneByName(sceneDataBase.playerScene.sceneName).IsValid())
            {
                SceneManager.LoadScene(sceneDataBase.playerScene.sceneName, LoadSceneMode.Additive);
                isPlayerLoaded = true;
            }
        }
    }

    public void LoadMainMenu()
    {
        //SceneManager.SetActiveScene(SceneManager.GetSceneByName(sceneDataBase.SystemScene.sceneName));

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        foreach (GameScene scene in sceneDataBase.mainMenuScenes)
        {
            // Check if the scene already loaded
            if (!SceneManager.GetSceneByName(scene.sceneName).IsValid())
            {
                SceneManager.LoadScene(scene.sceneName, LoadSceneMode.Additive);
            }
        }


        // Check if the scene already loaded
        if (SceneManager.GetSceneByName(sceneDataBase.playerScene.sceneName).IsValid())
        {
            SceneManager.UnloadSceneAsync(sceneDataBase.playerScene.sceneName);
        }



        // We unload the world scene and load the main menu
        foreach (GameScene scene in sceneDataBase.worldScenes)
        {
            // Check if the scene already loaded
            if (SceneManager.GetSceneByName(scene.sceneName).IsValid())
            {
                SceneManager.UnloadSceneAsync(scene.sceneName);
            }
        }

        isLoadingCompleted = false;
        isPlayerLoaded = false;
        worldHasLoaded = false;
    }

    public void LoadWorld()
    {
        // Hide menu
        //Show Loading Screen
        loadingScreen.SetActive(true);
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        foreach (GameScene scene in sceneDataBase.worldScenes)
        {
            // Check if the scene already loaded
            if (!SceneManager.GetSceneByName(scene.sceneName).IsValid())
            {
                // Add the scene to the async operation list
                scenesToLoad.Add(SceneManager.LoadSceneAsync(scene.sceneName, LoadSceneMode.Additive));
            }
        }
        StartCoroutine(LoadingScreen());
        //SaveSystem.Instance.Load();

        // We unload the main menu scene
        foreach (GameScene scene in sceneDataBase.mainMenuScenes)
        {
            // Check if the scene already loaded
            if (SceneManager.GetSceneByName(scene.sceneName).IsValid())
            {
                SceneManager.UnloadSceneAsync(scene.sceneName);
            }
        }


    }
    IEnumerator LoadingScreen()
    {
        float totalProgress = 0f;

        for (int i = 0; i < scenesToLoad.Count; i++)
        {
            while (!scenesToLoad[i].isDone)
            {
                totalProgress += scenesToLoad[i].progress / 2;
                Debug.Log(totalProgress + "/" + scenesToLoad.Count);
                Debug.Log((totalProgress / scenesToLoad.Count));
                slider.value = (totalProgress / scenesToLoad.Count);
                loadingProgressText.text = (int)(totalProgress * 100f / scenesToLoad.Count) + "%";

                yield return null;
            }
        }

        // Hide loading screen
        worldHasLoaded = true;
    }

    // called second
    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        foreach (GameObject item in scene.GetRootGameObjects())
        {
            if (item.CompareTag("SceneWorld"))
            {
                item.SetActive(false);
            }
        }
        Debug.Log("OnSceneLoaded: " + scene.name); 
    }

    public void ExitGame()
    {
        Application.Quit();
    }

    public void Spawn()
    {
        SpawnScript sc = FindObjectOfType<SpawnScript>();
        if (sc != null)
        {
            SaveSystem.Instance.Load();
            sc.Spawn();

            loadingScreen.SetActive(false);
            isLoadingCompleted = true;
        }
    }

}
