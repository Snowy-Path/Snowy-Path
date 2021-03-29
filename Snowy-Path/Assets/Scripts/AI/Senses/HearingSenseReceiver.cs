using UnityEngine;

/// <summary>
/// Receiver script of the Hearing Sense. The receiver is the AI itself.
/// Must be in a specific gameobject child of the AI with a Collider set on isTrigger, a Rigidbody set on isKinematic and with the Layer HearingSense.
/// </summary>
[RequireComponent(typeof(Collider))]
public class HearingSenseReceiver : MonoBehaviour {

    [Tooltip("The wolf controller. Used to tell the AI it heard a sound and the source (position) of this sound.")]
    public WolfController agent;

    /// <summary>
    /// Simple transfer to the agent the position of the sound heard.
    /// </summary>
    /// <param name="position"></param>
    public void Receive(Vector3 position) {
        agent.LastPosition = position;
    }

}
