using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

/// <summary>
/// Represent the Clothes prefabs creation menu under "GameObjects > Clothes".
/// It holds every pre-defined Clothes prefab instantiation methods.
/// </summary>
public class ClothCreationMenu : EditorWindow {

    /// <summary>
    /// Instantiate a Light Cloth Prefab with the Interactable script.
    /// It also holds a simple "PickableCloth" script to swap the cloth held in the first Inventory script found in the scene.
    /// </summary>
    /// <param name="menuCommand"></param>
    [MenuItem("GameObject/Clothes/Light Cloth", false, 10)]
    public static void CreateLightCloth(MenuCommand menuCommand) {

        // LightCloth prefab
        GameObject go = (GameObject)PrefabUtility.InstantiatePrefab(AssetDatabase.LoadAssetAtPath("Assets/Prefabs/Clothes/Light_Cloth.prefab", typeof(GameObject)));

        // Position
        MenuUtility.PlaceInFrontOfCamera(go);

        // Scene utility
        GameObjectUtility.SetParentAndAlign(go, menuCommand.context as GameObject);

        // Last child of parent / Bottom of hierarchy
        go.transform.SetAsLastSibling();

        Undo.RegisterCreatedObjectUndo(go, "Create " + go.name);
        Selection.activeObject = go;

    }

    /// <summary>
    /// Instantiate a Medium Cloth Prefab with the Interactable script.
    /// It also holds a simple "PickableCloth" script to swap the cloth held in the first Inventory script found in the scene.
    /// </summary>
    /// <param name="menuCommand"></param>
    [MenuItem("GameObject/Clothes/Medium Cloth", false, 20)]
    public static void CreateMediumCloth(MenuCommand menuCommand) {

        // LightCloth prefab
        GameObject go = (GameObject)PrefabUtility.InstantiatePrefab(AssetDatabase.LoadAssetAtPath("Assets/Prefabs/Clothes/Medium_Cloth.prefab", typeof(GameObject)));

        // Position
        MenuUtility.PlaceInFrontOfCamera(go);

        // Scene utility
        GameObjectUtility.SetParentAndAlign(go, menuCommand.context as GameObject);

        // Last child of parent / Bottom of hierarchy
        go.transform.SetAsLastSibling();

        Undo.RegisterCreatedObjectUndo(go, "Create " + go.name);
        Selection.activeObject = go;

    }

    /// <summary>
    /// Instantiate a Heavy Cloth Prefab with the Interactable script.
    /// It also holds a simple "PickableCloth" script to swap the cloth held in the first Inventory script found in the scene.
    /// </summary>
    /// <param name="menuCommand"></param>
    [MenuItem("GameObject/Clothes/Heavy Cloth", false, 30)]
    public static void CreateHeavyCloth(MenuCommand menuCommand) {

        // LightCloth prefab
        GameObject go = (GameObject)PrefabUtility.InstantiatePrefab(AssetDatabase.LoadAssetAtPath("Assets/Prefabs/Clothes/Heavy_Cloth.prefab", typeof(GameObject)));

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
