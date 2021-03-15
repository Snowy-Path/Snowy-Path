using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.Events;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// Represent the Dynampic system prefabs creation menu under "GameObjects > DynamicSystem".
/// It holds every dynamic tools prefabs instantiation methods.
/// </summary>
public class DynamicSystemCreationMenu : EditorWindow {

    /// <summary>
    /// Menu field. Makes all the selected gameobjects Dynamic.
    /// </summary>
    /// <param name="menuCommand"></param>
    [MenuItem("GameObject/DynamicSystem/Make Dynamic", false, 10)]
    public static void MakeDynamic(MenuCommand menuCommand) {

        for (int i = 0; i < Selection.gameObjects.Length; i++) {
            MakeGameObjectDynamic(Selection.gameObjects[i]);
        }

    }

    /// <summary>
    /// Validation method to enable the previous MakeDynamic method if and only if at least one object is selected.
    /// In this method we can add more condition.
    /// </summary>
    /// <returns>False if no transform is selected. True otherwise.</returns>
    [MenuItem("GameObject/DynamicSystem/Make Dynamic", true)]
    private static bool ValidateMakeDynamic() {
        // Return false if no transform is selected.

        return Selection.activeTransform != null;
    }

    /// <summary>
    /// Makes the gameobject go an DynamicComponent object.
    /// It adds the DynamicComponent scripts
    /// </summary>
    /// <param name="go">The modified gameobject.</param>
    private static void MakeGameObjectDynamic(GameObject go) {
        if (go.GetComponent<Interactable>() == null) {
            InteractableCreationMenu.MakeGameObjectInteractable(go);
        }
        Interactable interactable = go.GetComponent<Interactable>();

        DynamicComponent dynamicComponent = (DynamicComponent)Undo.AddComponent(go, typeof(DynamicComponent));

        // Show Interaction Feedback
        if (interactable.onInteract == null)
            interactable.onInteract = new UnityEvent();
        UnityAction sendEventMethodDelegate = System.Delegate.CreateDelegate(typeof(UnityAction), dynamicComponent, "SendDynamicEvent") as UnityAction;
        UnityEventTools.AddPersistentListener(interactable.onInteract, sendEventMethodDelegate);
    }


    [MenuItem("GameObject/DynamicSystem/DynamicSet", false, 30)]
    public static void CreateDynamicSet(MenuCommand menuCommand) {
        // Prefab
        GameObject go = new GameObject("DynamicSet");
        DynamicSet dynSet = go.AddComponent<DynamicSet>();

        //Add child objects
        GameObject defaultSet = new GameObject("Default Set");
        GameObject alternativeSet = new GameObject("Alternative Set");

        //Link child set into DynamicSet
        dynSet.defaultSet = defaultSet;
        dynSet.alternativeSet = alternativeSet;

        GameObjectUtility.SetParentAndAlign(defaultSet, go);
        GameObjectUtility.SetParentAndAlign(alternativeSet, go);


        // Position
        PlaceInScene(go, menuCommand.context);
        go.transform.SetAsLastSibling();
    }

    /// <summary>
    /// Instantiate a Prefab with the Interactable script.
    /// </summary>
    /// <param name="menuCommand"></param>
    [MenuItem("GameObject/DynamicSystem/SphereDynamicTrigger", false, 50)]
    public static void CreateSphereDynamicTrigger(MenuCommand menuCommand) {

        // Gameobject & name
        GameObject go = new GameObject("SphereDynamicTrigger");
        go.AddComponent<DynamicTrigger>();
        SphereCollider col = go.AddComponent<SphereCollider>();
        col.isTrigger = true;

        // Complete go with needed scripts
        PlaceInScene(go, menuCommand.context);
    }

    /// <summary>
    /// Instantiate a Prefab with the DynamicTrigger script.
    /// </summary>
    /// <param name="menuCommand"></param>
    [MenuItem("GameObject/DynamicSystem/BoxDynamicTrigger", false, 50)]
    public static void CreateBoxDynamicTrigger(MenuCommand menuCommand) {

        // Gameobject & name
        GameObject go = new GameObject("BoxDynamicTrigger");
        go.AddComponent<DynamicTrigger>();
        BoxCollider col = go.AddComponent<BoxCollider>();
        col.isTrigger = true;

        // Complete go with needed scripts
        PlaceInScene(go, menuCommand.context);
    }

    [MenuItem("GameObject/DynamicSystem/DynamicInteractable", false, 60)]
    public static void CreateDynamicInteractable(MenuCommand menuCommand) {

        // Prefab
        GameObject go = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        go.name = "DynamicInteractable";
        InteractableCreationMenu.MakeGameObjectInteractable(go);
        MakeGameObjectDynamic(go);

        PlaceInScene(go, menuCommand.context);
    }


    /// <summary>
    /// Place an object in scene
    /// </summary>
    /// <param name="go">The modified gameobject.</param>
    private static void PlaceInScene(GameObject go, Object context) {

        // Position
        MenuUtility.PlaceInFrontOfCamera(go);

        // Scene utility
        GameObjectUtility.SetParentAndAlign(go, context as GameObject);

        Undo.RegisterCreatedObjectUndo(go, "Create " + go.name);
        Selection.activeObject = go;
    }
}
