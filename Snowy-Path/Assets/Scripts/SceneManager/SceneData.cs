using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

[CreateAssetMenu(fileName = "sceneDB", menuName = "Scene Data/Database")]
public class ScenesData : ScriptableObject
{
    public List<GameScene> worldScenes = new List<GameScene>();
    public List<GameScene> mainMenuScenes = new List<GameScene>();
}