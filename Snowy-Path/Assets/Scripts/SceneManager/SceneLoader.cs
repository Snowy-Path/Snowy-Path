using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Playables;
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
    bool levelChange = false;

    public List<LevelLoader> levelLoadersActive;


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
        levelLoadersActive = new List<LevelLoader>();

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

        if(worldHasLoaded && !isPlayerLoaded)
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
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        // Check if the scene already loaded
        if (!SceneManager.GetSceneByName(sceneDataBase.mainMenuScenes.sceneName).IsValid())
        {
            SceneManager.LoadScene(sceneDataBase.mainMenuScenes.sceneName, LoadSceneMode.Additive);
        }

        // Unloading old level sequence

        // Unload Active level
        Scene activeScene = SceneManager.GetActiveScene();

        // Check if the scene already loaded
        if (activeScene.IsValid())
        {
            SceneManager.UnloadSceneAsync(activeScene);
        }

        // Unload Player
        // Check if the scene already loaded
        if (SceneManager.GetSceneByName(sceneDataBase.playerScene.sceneName).IsValid())
        {
            SceneManager.UnloadSceneAsync(sceneDataBase.playerScene.sceneName);
        }



        //// We unload the world scene and load the main menu
        //foreach (GameScene scene in sceneDataBase.worldScenes)
        //{
        //    // Check if the scene already loaded
        //    if (SceneManager.GetSceneByName(scene.sceneName).IsValid())
        //    {
        //        SceneManager.UnloadSceneAsync(scene.sceneName);
        //    }
        //}

        isLoadingCompleted = false;
        isPlayerLoaded = false;
        worldHasLoaded = false;
    }

    public void LoadLevel(string sceneToLoadName)
    {
        loadingScreen.SetActive(true);

        if (SceneManager.GetSceneByName(sceneToLoadName).IsValid())
        {
            levelChange = true;
            worldHasLoaded = true;
            LevelChanged();

            return;
        }

        levelChange = true;

        // Check if the scene already loaded
        if (!SceneManager.GetSceneByName(sceneToLoadName).IsValid())
        {
            // Add the scene to the async operation list
            scenesToLoad.Add(SceneManager.LoadSceneAsync(sceneToLoadName, LoadSceneMode.Additive));
        }

        // Unloading old level sequence

        Scene activeScene = SceneManager.GetActiveScene();

        // Check if the scene already loaded
        if (activeScene.IsValid())
        {
            SceneManager.UnloadSceneAsync(activeScene);
        }

        StartCoroutine(LoadingScreen());
    }

    public void LoadWorld()
    {
        // Hide menu
        //Show Loading Screen
        loadingScreen.SetActive(true);
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        SaveSystem.Instance.Load();

        SceneSave sceneSave = FindObjectOfType<SceneSave>();
        if(sceneSave != null)
        {
            if (sceneSave.SceneName == "")
            {
                // Check if the scene already loaded
                if (!SceneManager.GetSceneByName(sceneDataBase.mainLevel.sceneName).IsValid())
                {
                    // Add the scene to the async operation list
                    scenesToLoad.Add(SceneManager.LoadSceneAsync(sceneDataBase.mainLevel.sceneName, LoadSceneMode.Additive));
                }
            }
            else
            {
                // Check if the scene already loaded
                if (!SceneManager.GetSceneByName(sceneSave.SceneName).IsValid())
                {
                    // Add the scene to the async operation list
                    scenesToLoad.Add(SceneManager.LoadSceneAsync(sceneSave.SceneName, LoadSceneMode.Additive));
                }
            }

            StartCoroutine(LoadingScreen());

            // We unload the main menu scene

            // Check if the scene already loaded
            if (SceneManager.GetSceneByName(sceneDataBase.mainMenuScenes.sceneName).IsValid())
            {
                SceneManager.UnloadSceneAsync(sceneDataBase.mainMenuScenes.sceneName);
            }
        }



    }

    /// <summary>
    /// Load the world via to a specific level
    /// This method was made just for a debug build not for release
    /// </summary>
    /// <param name="sceneToLoad"></param>
    public void LoadWorld(GameScene sceneToLoad)
    {
        // Hide menu
        //Show Loading Screen
        loadingScreen.SetActive(true);
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        SaveSystem.Instance.Load();

        // Check if the scene already loaded
        if (!SceneManager.GetSceneByName(sceneToLoad.sceneName).IsValid())
        {
            // Add the scene to the async operation list
            scenesToLoad.Add(SceneManager.LoadSceneAsync(sceneToLoad.sceneName, LoadSceneMode.Additive));
        }

        StartCoroutine(LoadingScreen());

        // We unload the main menu scene

        // Check if the scene already loaded
        if (SceneManager.GetSceneByName(sceneDataBase.mainMenuScenes.sceneName).IsValid())
        {
            SceneManager.UnloadSceneAsync(sceneDataBase.mainMenuScenes.sceneName);
        }
    }


    //public void LoadWorld()
    //{
    //    // Hide menu
    //    //Show Loading Screen
    //    loadingScreen.SetActive(true);
    //    Cursor.lockState = CursorLockMode.Locked;
    //    Cursor.visible = false;

    //    //foreach (GameScene scene in sceneDataBase.worldScenes)
    //    //{
    //    //    // Check if the scene already loaded
    //    //    if (!SceneManager.GetSceneByName(scene.sceneName).IsValid())
    //    //    {
    //    //        // Add the scene to the async operation list
    //    //        scenesToLoad.Add(SceneManager.LoadSceneAsync(scene.sceneName, LoadSceneMode.Additive));
    //    //    }
    //    //}

    //    StartCoroutine(LoadingScreen());

    //    // We unload the main menu scene

    //    // Check if the scene already loaded
    //    if (SceneManager.GetSceneByName(sceneDataBase.mainMenuScenes.sceneName).IsValid())
    //    {
    //        SceneManager.UnloadSceneAsync(sceneDataBase.mainMenuScenes.sceneName);
    //    }
    //}

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
        if(scene.name == sceneDataBase.mainMenuScenes.sceneName)
        {
            SceneManager.SetActiveScene(scene);
            return;
        }

        foreach (GameObject item in scene.GetRootGameObjects())
        {
            if (item.CompareTag("SceneWorld"))
            {
                SceneManager.SetActiveScene(scene);
                item.SetActive(true);
            }
        }

        if (levelChange)
        {
            levelChange = false;
            LevelChanged();
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

    public void LevelChanged()
    {

        CharacterController charController = FindObjectOfType<CharacterController>();
        charController.enabled = false;
        SpawnPlayerPosition spawn = null;
        Scene activeScene = SceneManager.GetActiveScene();
        foreach (GameObject item in activeScene.GetRootGameObjects())
        {
            if (item.GetComponent<SpawnPlayerPosition>() != null)
            {
                spawn = item.GetComponent<SpawnPlayerPosition>();
                if(spawn.defaultLight != null)
                {
                    LightTransition.LightTransitionTo(spawn.defaultLight);
                }
            }
        }
        if(spawn != null)
        {
            charController.transform.position = spawn.transform.position;

        }
        else
        {
            Debug.LogError("SpawnPlayerPosition not found, character will be put at (0,0,0)");
            charController.transform.position = new Vector3(0,0,0);
        }

        charController.transform.eulerAngles = new Vector3(14, -104, -83);
        charController.enabled = true;

        PlayerPlayable playerPlayable = FindObjectOfType<PlayerPlayable>();
        loadingScreen.SetActive(false);
        if (playerPlayable != null)
        {
            playerPlayable.playableWakeup.Play();
        }
    }

}
