using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class SaveInteractable : MonoBehaviour
{

    public PlayableDirector playableDeath;
    public PlayableDirector playableWakeup;

    public void Start()
    {
        playableDeath.stopped += OnPlayableDirectorStopped;
        playableWakeup.stopped += OnPlayableDirectorStopped;
    }
    public void LaunchSave() {
        SaveSystem.Instance.Save();
    }

    void OnPlayableDirectorStopped(PlayableDirector aDirector)
    {
        if (playableWakeup == aDirector)
        {
            this.transform.eulerAngles = new Vector3(0, 0, this.transform.rotation.z);
        }
    }
}
