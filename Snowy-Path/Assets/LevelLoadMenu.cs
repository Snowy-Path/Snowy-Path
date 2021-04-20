using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelLoadMenu : MonoBehaviour
{


    public void LoadScene(GameScene scene)
    {
        SceneLoader.Instance.LoadLevel(scene.sceneName);
    }
}
