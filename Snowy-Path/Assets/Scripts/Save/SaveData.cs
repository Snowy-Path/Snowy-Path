using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class SaveData
{
    public Dictionary<string, object> state;

    public SaveData() {
        state = new Dictionary<string, object>();
    }
}
