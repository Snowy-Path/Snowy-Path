using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SceneLoader : MonoBehaviour
{
    public ScenesData sceneDataBase;
    public GameObject loadingScreen;
    public Slider slider;
    public Text loadingProgressText;
    public static SceneLoader Instance;
    private List<AsyncOperation> scenesToLoad = new List<AsyncOperation>();

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

    public void LoadMainMenu()
    {
        foreach (GameScene scene in sceneDataBase.mainMenuScenes)
        {
            // Check if the scene already loaded
            if (!SceneManager.GetSceneByName(scene.sceneName).IsValid())
            {
                SceneManager.LoadScene(scene.sceneName, LoadSceneMode.Additive);
            }
        }
    }

    public void LoadWorld()
    {
        // Hide menu
        //Show Loading Screen
        loadingScreen.SetActive(true);

        foreach (GameScene scene in sceneDataBase.worldScenes)
        {
            // Check if the scene already loaded
            if (!SceneManager.GetSceneByName(scene.sceneName).IsValid())
            {
                // Add the scene to the async operation list
                scenesToLoad.Add(SceneManager.LoadSceneAsync(scene.sceneName, LoadSceneMode.Additive));
                //SceneManager.LoadScene(scene.sceneName, LoadSceneMode.Additive);
            }
        }
        StartCoroutine(LoadingScreen());
        //SaveSystem.Instance.Load();

    }
    IEnumerator LoadingScreen()
    {
        float totalProgress = 0f;

        for (int i = 0; i < scenesToLoad.Count; i++)
        {
            while (!scenesToLoad[i].isDone)
            {
                totalProgress += scenesToLoad[i].progress;
                slider.value = totalProgress / scenesToLoad.Count;
                loadingProgressText.text = totalProgress * 100f + "%";

                yield return null;
            }
        }

        // Hide loading screen
        loadingScreen.SetActive(false);
        SaveSystem.Instance.Load();
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

}
