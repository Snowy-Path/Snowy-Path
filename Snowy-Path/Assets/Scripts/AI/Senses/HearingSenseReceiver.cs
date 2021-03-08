using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HearingSenseReceiver : MonoBehaviour {

    public WolfController agent;

    public void Receive(Vector3 position) {
        agent.SoundPosition = position;
    }

}
