using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCampfireSave : MonoBehaviour, ISaveable {

    [SerializeField] string lastCampfireId = "";

    public string LastCampfireId { get => lastCampfireId; set => lastCampfireId = value; }

    public void RestorePlayerAtCampfire() {
        // We go through all campfire in scene
        foreach  (Campfire item in FindObjectsOfType<Campfire>()) {
            // Check if it's the same campfire as the save
            if(item.Id == lastCampfireId) {
                // We go through all the campfire object to find the transform respawn point
                foreach (Transform respawnTransform in item.gameObject.GetComponentsInChildren<Transform>()) {
                    // If found the player transform is set as the last campfire respawn point
                    if (respawnTransform.gameObject.CompareTag("PlayerRespawnPoint")) {
                        CharacterController charController = GetComponent<CharacterController>();
                        charController.enabled = false;
                        this.transform.SetPositionAndRotation(respawnTransform.position, respawnTransform.rotation);
                        charController.enabled = true;

                    }
                }
            }
        }
    }

    #region Save section
    public object CaptureState() {
        return new PlayerCampfireData
        {
            id = LastCampfireId
        };
    }

    public void RestoreState(object state) {
        var saveData = (PlayerCampfireData)state;

        lastCampfireId = saveData.id;
        RestorePlayerAtCampfire();
    }

    [Serializable]
    private struct PlayerCampfireData {
        public string id;
    }

    #endregion
}
