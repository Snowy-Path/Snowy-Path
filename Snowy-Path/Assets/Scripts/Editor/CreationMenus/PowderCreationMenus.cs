using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

/// <summary>
/// Represent the Powder prefabs creation menu under "GameObjects > Powders".
/// It holds every pre-defined Powder prefabs instantiation methods.
/// </summary>
public class PowderCreationMenu {

    /// <summary>
    /// Instantiate a Powder Prefab with the Interactable script.
    /// </summary>
    /// <param name="menuCommand"></param>
    [MenuItem("GameObject/Powder", false, 10)]
    public static void CreatePowder(MenuCommand menuCommand) {

        // LightCloth prefab
        GameObject go = (GameObject)PrefabUtility.InstantiatePrefab(AssetDatabase.LoadAssetAtPath("Assets/Prefabs/Powders/Powder.prefab", typeof(GameObject)));

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
