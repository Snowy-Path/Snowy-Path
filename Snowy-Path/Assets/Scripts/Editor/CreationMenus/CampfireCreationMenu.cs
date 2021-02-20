﻿using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

/// <summary>
/// Represent the Campfire prefab creation menu under "GameObjects".
/// It holds every pre-defined Campfire prefab instantiation methods.
/// </summary>
public class CampfireCreationMenu {

    /// <summary>
    /// Instantiate a Campfire Prefab with the Interactable script.
    /// </summary>
    /// <param name="menuCommand"></param>
    [MenuItem("GameObject/Campfire/Campfire", false, 10)]
    public static void CreateCampfire(MenuCommand menuCommand) {

        // LightCloth prefab
        GameObject go = (GameObject)PrefabUtility.InstantiatePrefab(AssetDatabase.LoadAssetAtPath("Assets/Prefabs/Campfire.prefab", typeof(GameObject)));

        // Position
        MenuUtility.PlaceInFrontOfCamera(go);

        // Scene utility
        GameObjectUtility.SetParentAndAlign(go, menuCommand.context as GameObject);

        Undo.RegisterCreatedObjectUndo(go, "Create " + go.name);
        Selection.activeObject = go;

    }
}
