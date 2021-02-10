using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{

    public static GameManager instance;

    // Player
    internal PlayerController player;

    private void Awake() {

        // Singleton of the GameManager
        if (instance == null) {
            instance = this;
        }
        else {
            Destroy(instance);
            instance = gameObject.GetComponent<GameManager>();
            return;
        }

        // 
        player = FindObjectOfType<PlayerController>();
    }
}
