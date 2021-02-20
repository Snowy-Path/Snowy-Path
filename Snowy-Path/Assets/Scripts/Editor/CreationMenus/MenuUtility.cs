using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class MenuUtility {

    /// <summary>
    /// Moves the created gameobject in the center of the viewport like a simple Cube creation does.
    /// </summary>
    /// <param name="go">The moved gameobject.</param>
    internal static void PlaceInFrontOfCamera(GameObject go) {

        float range = 50.0f;

        // Camera Position
        Camera sceneCam = SceneView.lastActiveSceneView.camera;
        Ray ray = new Ray(sceneCam.transform.position, sceneCam.transform.forward);

        RaycastHit info;
        if (Physics.Raycast(ray, out info, range)) {
            go.transform.position = info.point;
        } else {
            go.transform.position = sceneCam.transform.position + sceneCam.transform.forward * range;
        }
    }

}
