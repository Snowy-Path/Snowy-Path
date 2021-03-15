using UnityEngine;

[CreateAssetMenu(fileName = "GameScene", menuName = "Scene Data/Scene")]
public class GameScene : ScriptableObject
{
    [Header("Information")]
    public string sceneName;
    public string shortDescription;
}
