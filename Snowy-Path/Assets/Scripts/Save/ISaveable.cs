using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ISaveable
{
    #region Save
    object CaptureState();
    void RestoreState(object state);
    #endregion
}
