using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class StateBase {



    public abstract void OnEntry();
    public abstract void OnExit();
    public abstract void OnUpdate();
    public abstract void OnFixedUpdate();
    public abstract void OnLateUpdate();

}
