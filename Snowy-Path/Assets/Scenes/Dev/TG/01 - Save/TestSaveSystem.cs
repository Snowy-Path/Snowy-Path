using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class TestSaveSystem : MonoBehaviour, ISaveable
{

    [SerializeField] public string dataVersion = "";
    [SerializeField] public int health = 0;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        var keyboard = Keyboard.current;
        if (keyboard == null) {
            Debug.LogError("No keyboard connected");
            return;
        }

        if (keyboard.yKey.wasPressedThisFrame) {
            SaveSystem.Instance.CreateNewSave(0);
        }

        if (keyboard.uKey.wasPressedThisFrame) {
            SaveSystem.Instance.CreateNewSave(1);
        }

        if (keyboard.f3Key.wasPressedThisFrame) {
            SaveSystem.Instance.Save();
        }

        if (keyboard.eKey.wasPressedThisFrame) {
            SaveSystem.Instance.Load();
        }

        if (keyboard.f1Key.wasPressedThisFrame) {
            SaveSystem.Instance.SetCurrentSave(0);
        }

        if (keyboard.f2Key.wasPressedThisFrame) {
            SaveSystem.Instance.SetCurrentSave(1);
        }
    }

    #region Save section
    public object CaptureState() {
        return new HeaderData
        {
            dataVersion = dataVersion,
            health = health
        };
    }

    public void RestoreState(object state) {
        var saveData = (HeaderData)state;

        dataVersion = saveData.dataVersion;
        health = saveData.health;
    }

    [Serializable]
    private struct HeaderData {
        public string dataVersion;
        public int health;
    }

    #endregion

    #region GUI

    private void OnGUI() {

        GUI.Box(new Rect(10, 10, 160, 220), "Save");

        GUI.Label(new Rect(20, 40, 140, 30), $"Dataversion : {dataVersion}");

        GUI.Label(new Rect(20, 70, 140, 30), $"CurrentSave : {SaveSystem.Instance.currentSave}");

    }
    #endregion


}
