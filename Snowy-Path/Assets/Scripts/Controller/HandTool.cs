using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class HandTool : MonoBehaviour
{
    public UnityEvent OnUse;

    public void UseTool() {
        OnUse.Invoke();
    }
}
