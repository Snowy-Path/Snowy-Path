using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

/// <summary>
/// Represent the Waypoint prefab creation menu under "GameObjects".
/// It holds every pre-defined Campfire prefab instantiation methods.
/// </summary>
public class WaypointCreationMenu : EditorWindow {

    /// <summary>
    /// Instantiate a Waypoint Prefab with the Interactable script.
    /// </summary>
    /// <param name="menuCommand"></param>
    [MenuItem("GameObject/Waypoint", false, 10)]
    public static void CreateWaypoint(MenuCommand menuCommand) {

        // LightCloth prefab
        GameObject go = (GameObject)PrefabUtility.InstantiatePrefab(AssetDatabase.LoadAssetAtPath("Assets/Prefabs/Ennemies/Waypoint.prefab", typeof(GameObject)));

        // Position
        MenuUtility.PlaceInFrontOfCamera(go);

        // Scene utility
        GameObjectUtility.SetParentAndAlign(go, menuCommand.context as GameObject);

        // Last child of parent / Bottom of hierarchy
        go.transform.SetAsLastSibling();

        Undo.RegisterCreatedObjectUndo(go, "Create " + go.name);
        Selection.activeObject = go;

    }
}
