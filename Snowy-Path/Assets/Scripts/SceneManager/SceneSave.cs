using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneSave : MonoBehaviour, ISaveable
{
    [SerializeField] string sceneName = "";

    public string SceneName { get => sceneName; set => sceneName = value; }

    #region Save section
    public object CaptureState()
    {
        return new SceneSaveData
        {
            sceneName = sceneName
        };
    }

    public void RestoreState(object state)
    {
        var saveData = (SceneSaveData)state;

        sceneName = saveData.sceneName;
    }

    [Serializable]
    private struct SceneSaveData
    {
        public string sceneName;
    }

    #endregion
}
