using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestPlayerSave : MonoBehaviour, ISaveable
{
    [SerializeField] public string namePlayer = "";

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public object CaptureState() {
        return new PlayerData
        {
            namePlayer = namePlayer,
            tx = transform.position.x,
            ty = transform.position.y,
            tz = transform.position.z
        };
    }

    public void RestoreState(object state) {
        var saveData = (PlayerData)state;

        namePlayer = saveData.namePlayer;
        transform.position = new Vector3(saveData.tx, saveData.ty, saveData.tz);
    }

    [Serializable]
    private struct PlayerData {
        public string namePlayer;
        public float tx;
        public float ty;
        public float tz;
    }
}
