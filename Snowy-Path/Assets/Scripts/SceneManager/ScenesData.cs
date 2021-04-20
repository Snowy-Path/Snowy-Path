using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

[CreateAssetMenu(fileName = "ScenesData", menuName = "Scene Data/Database")]
public class ScenesData : ScriptableObject
{
    public GameScene mainLevel;
    public GameScene mainMenuScenes;
    public GameScene playerScene;
    public GameScene SystemScene;
}