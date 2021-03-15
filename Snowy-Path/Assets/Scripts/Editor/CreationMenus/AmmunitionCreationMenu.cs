using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

/// <summary>
/// Represent the Ammunition prefabs creation menu under "GameObjects > Ammunitions".
/// It holds every pre-defined Ammunition prefabs instantiation methods.
/// </summary>
public class AmmunitionCreationMenu : EditorWindow {
    /// <summary>
    /// Instantiate a Ammunition Box Prefab with the Interactable script.
    /// An ammo box can be used an infinite amount of time and refills the player's ammunition to max every time.
    /// </summary>
    /// <param name="menuCommand"></param>
    [MenuItem("GameObject/Ammunition/Box", false, 10)]
    public static void CreateAmmunitionBox(MenuCommand menuCommand) {

        // LightCloth prefab
        GameObject go = (GameObject)PrefabUtility.InstantiatePrefab(AssetDatabase.LoadAssetAtPath("Assets/Prefabs/Ammunitions/AmmoBox.prefab", typeof(GameObject)));

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
    /// Instantiate a Ammunition Pouch Prefab with the Interactable script.
    /// An ammo pouch can be used only once and refills the player's to the number of ammo stored in the pouch itself.
    /// </summary>
    /// <param name="menuCommand"></param>
    [MenuItem("GameObject/Ammunition/Pouch", false, 10)]
    public static void CreateAmmunitionPouch(MenuCommand menuCommand) {

        // LightCloth prefab
        GameObject go = (GameObject)PrefabUtility.InstantiatePrefab(AssetDatabase.LoadAssetAtPath("Assets/Prefabs/Ammunitions/AmmoPouch.prefab", typeof(GameObject)));

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
