using UnityEngine;

/// <summary>
/// Seeing sense of an AI.
/// Must be in a specific gameobject child of the AI with a Collider set on isTrigger, a Rigidbody set on isKinematic and with the Layer SeeingSense.
/// The Player MUST have a specific gameobject child with a collider and the Layer set to SeeingSense too. 
/// </summary>
[RequireComponent(typeof(Collider))]
public class SeeingSense : MonoBehaviour {

    [Tooltip("The wolf controller. Used to tell the AI it is seeing a player and where he is seeing it.")]
    public WolfController agent;

    [Tooltip("Layers to detect when the ray is cast. Allows to ignore layers.")]
    public LayerMask layers;

    /// <summary>
    /// When a collider enters, it compares if the other collider tag match the targeted tag.
    /// If the tag matches, cast a ray in the direction of the other collider in order to detect if there is another object blocking the view.
    /// If there is nothing blocking the view, the AI knows the position and the property <c>IsSeeingPlayer</c> is set to true.
    /// </summary>
    /// <param name="other"></param>
    private void OnTriggerStay(Collider other) {

        if (!other.CompareTag("Player")) {
            return;
        }

        Vector3 origin = transform.position;
        Vector3 destination = other.transform.position;
        Vector3 direction = destination - origin;
        float length = direction.magnitude;
        direction.Normalize();

        Ray ray = new Ray(origin, direction);

        RaycastHit[] hits = Physics.RaycastAll(ray, length, layers);

        agent.IsSeeingPlayer = (hits.Length == 1);

        if (agent.IsSeeingPlayer) {
            agent.LastPosition = other.transform.position;
        }

        Debug.DrawRay(ray.origin, ray.direction * length, agent.IsSeeingPlayer ? Color.green : Color.red);
    }

    /// <summary>
    /// Resets the <c>IsSeeingPlayer</c> property. Ensure it doesn't see the player anymore if it is out of range.
    /// </summary>
    /// <param name="other"></param>
    private void OnTriggerExit(Collider other) {
        agent.IsSeeingPlayer = false;
    }

}
