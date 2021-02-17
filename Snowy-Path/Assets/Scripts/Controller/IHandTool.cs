using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public interface IHandTool {
    void PrimaryUse();
    void ToggleDisplay(bool display);

}

public enum EToolType {
    None,
    Compass,
    Pistol,
    Scope,
    Torch
}
