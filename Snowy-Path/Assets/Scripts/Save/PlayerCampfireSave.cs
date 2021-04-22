using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerCampfireSave : MonoBehaviour, ISaveable {

    [SerializeField] string lastCampfireId = "";
    [SerializeField] string sceneName = "";

    public string LastCampfireId { get => lastCampfireId; set => lastCampfireId = value; }
    public string SceneName { get => sceneName; set => sceneName = value; }

    public void RestorePlayerAtCampfire() {
        if(lastCampfireId == "")
        {
            CharacterController charController = GetComponent<CharacterController>();
            PlayerController playerController = GetComponent<PlayerController>();
            charController.enabled = false;
            playerController.enabled = false;
            this.transform.position = FindObjectOfType<SpawnPlayerPosition>().transform.position;
            this.transform.eulerAngles = new Vector3(0, 0, 0);
            this.transform.eulerAngles = new Vector3(14, -104, -83);
        }
        else
        {
            // We go through all campfire in scene
            foreach (Campfire item in FindObjectsOfType<Campfire>())
            {
                // Check if it's the same campfire as the save
                if (item.Id == lastCampfireId)
                {
                    // We go through all the campfire object to find the transform respawn point
                    foreach (Transform respawnTransform in item.gameObject.GetComponentsInChildren<Transform>())
                    {
                        // If found the player transform is set as the last campfire respawn point
                        if (respawnTransform.gameObject.CompareTag("PlayerRespawnPoint"))
                        {
                            if (item.lightForThisCampfire != null)
                            {
                                LightTransition.LightTransitionTo(item.lightForThisCampfire);
                            }
                            CharacterController charController = GetComponent<CharacterController>();
                            PlayerController playerController = GetComponent<PlayerController>();
                            charController.enabled = false;
                            playerController.enabled = false;
                            this.transform.position = respawnTransform.position;
                            this.transform.eulerAngles = new Vector3(0, 0, 0);
                            this.transform.eulerAngles = new Vector3(14, -104, -83);
                            //charController.enabled = true;
                        }
                    }
                }
            }
        }
    }

    #region Save section
    public object CaptureState() {
        return new PlayerCampfireData
        {
            id = LastCampfireId,
            sceneName = sceneName
        };
    }

    public void RestoreState(object state) {
        var saveData = (PlayerCampfireData)state;

        lastCampfireId = saveData.id;
        sceneName = saveData.sceneName;
    }

    [Serializable]
    private struct PlayerCampfireData {
        public string id;
        public string sceneName;
    }

    #endregion
}
