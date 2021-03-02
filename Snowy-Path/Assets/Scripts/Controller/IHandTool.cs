using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public interface IHandTool {
    EToolType ToolType { get;}
    void StartPrimaryUse();
    void CancelPrimaryUse();
    void SecondaryUse();
    void ToggleDisplay(bool display);
}

public enum EToolType {
    None,
    MapCompass,
    Pistol,
    Scope,
    Torch
}
