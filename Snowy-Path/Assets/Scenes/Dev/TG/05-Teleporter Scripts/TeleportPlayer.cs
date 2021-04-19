using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TeleportPlayer : MonoBehaviour
{
    //public Vector3 teleportPosition = new Vector3(0,0,0);
    //public bool tpToRespawn = false;
    public GameScene sceneToLoad;
    TextMeshProUGUI text;

    public void Start()
    {
        if (sceneToLoad != null)
        {
            text = GetComponentInChildren<TextMeshProUGUI>();
            if(text != null)
            {
                text.text = sceneToLoad.sceneName;
            }
        }
    }

    public void Teleport() {

        SceneLoader.Instance.LoadLevel(sceneToLoad.sceneName);

        //CharacterController charController = FindObjectOfType<CharacterController>();
        //charController.enabled = false;
        //if (tpToRespawn)
        //{
        //    charController.transform.position = FindObjectOfType<SpawnPlayerPosition>().transform.position;
        //}
        //else
        //{
        //    charController.transform.position = teleportPosition;
        //}
        //charController.enabled = true;
    }
}
