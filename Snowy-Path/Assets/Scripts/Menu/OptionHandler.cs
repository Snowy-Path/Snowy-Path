using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class OptionHandler : MonoBehaviour {

    public static OptionHandler Instance;

    public OptionSettings optionSettings;

    public bool Gamepadconnected { get; private set; }
    public float Sensitivity { get => optionSettings.sensitivity; }

    private Resolution[] resolutions;

    // Start is called before the first frame update
    void Awake() {
        // Singleton of the GameManager
        if (Instance == null) {
            Instance = this;
        }
        else {
            Destroy(Instance);
            Instance = gameObject.GetComponent<OptionHandler>();
            return;
        }

        List<string> options = new List<string>();
        resolutions = Screen.resolutions;
        int currentResolutionIndex = 0;

        for (int i = 0; i < resolutions.Length; i++) {

            string option = resolutions[i].width + " x " + resolutions[i].height;
            options.Add(option);

            if (resolutions[i].width == Screen.currentResolution.width && resolutions[i].height == Screen.currentResolution.height)
                currentResolutionIndex = i;
        }

        // If no settings save already, loading default settings
        if (!OptionSave.Init()) {

            optionSettings = new OptionSettings
            {
                MasterVolume = 1f,
                MusicVolume = 0.5f,
                SFXVolume = 0.5f,
                resolution_index = currentResolutionIndex,
                aa_index = 2,
                gammavalue = 1
            };
            OptionSave.Save(optionSettings);
        }
        else {
            optionSettings = OptionSave.Load();
        }
    }

    private void FixedUpdate() {
        Gamepadconnected = Gamepad.current != null;
    }
}

// Update is called once per frame

