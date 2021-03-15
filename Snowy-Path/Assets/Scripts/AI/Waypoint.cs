using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Represent a Waypoint in the scene.
/// A Waypoint holds a list of next waypoints that is chosen randomly.
/// </summary>
public class Waypoint : MonoBehaviour {

    [Tooltip("Hold a list of waypoints to go next. If empty, the Ai will choose the default waypoint.")]
    public Waypoint[] nextWaypoints;
    
    /// <summary>
    /// Choose and return randomly the next waypoint.
    /// </summary>
    /// <returns></returns>
    public Waypoint GetNextWaypoint() {
        if (nextWaypoints.Length == 0) {
            return null;
        }
        return nextWaypoints[Random.Range(0, nextWaypoints.Length)];
    }

}
