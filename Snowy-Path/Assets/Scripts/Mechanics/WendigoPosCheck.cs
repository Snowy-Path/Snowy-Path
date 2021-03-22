using UnityEngine;

/// <summary>
/// Checks if the Wendigo needs to be teleported closer to the player.
/// </summary>
[RequireComponent(typeof(Collider))]
public class WendigoPosCheck : MonoBehaviour {

    [Tooltip("Teleportation point.")]
    [SerializeField] private Transform newWendigoPos;

    [Tooltip("Distance to check. If the Wendigo of the Player than this distance, the Wendigo is teleported.")]
    [Min(0)]
    [SerializeField] private float distanceThreshold = 10;

    /// <summary>
    /// Triggered when another collider with Layer 'PlayerBody' enters this one.
    /// Check the distance between the Player and the Wendigo. Teleport sthe Wendigo if needed.
    /// </summary>
    /// <param name="other"></param>
    private void OnTriggerEnter(Collider other) {

        WendigoController wendigo = FindObjectOfType<WendigoController>();

        if (wendigo) {
            if (other.CompareTag("Player")) { // Useful with the layer matrix ?

                if (Vector3.Distance(wendigo.transform.position, other.transform.position) > distanceThreshold) {
                    wendigo.Teleport(newWendigoPos);
                }
                Destroy(gameObject);
            }
        } else {
            Debug.LogError("Can't find Wendigo to teleport.");
        }
    }

    /// <summary>
    /// Draw the distance threshold as a yellow wire sphere.
    /// </summary>
    private void OnDrawGizmos() {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, distanceThreshold);
    }
}

