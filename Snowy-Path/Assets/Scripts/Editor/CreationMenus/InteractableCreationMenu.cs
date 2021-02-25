using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.Events;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// Represent the Interactable gameobject creation menu under "GameObjects > Interactable".
/// It holds every pre-defined Interactable asset creation methods.
/// </summary>
public class InteractableCreationMenu {

    #region Make Interactable

    /// <summary>
    /// Menu field. Makes all the selected gameobjects Interactable.
    /// </summary>
    /// <param name="menuCommand"></param>
    [MenuItem("GameObject/Interaction/Make Interactable", false, 10)]
    public static void MakeInteractable(MenuCommand menuCommand) {

        for (int i = 0; i < Selection.gameObjects.Length; i++) {
            MakeGameObjectInteractable(Selection.gameObjects[i]);
        }

    }

    /// <summary>
    /// Validation method to enable the previous MakeInteractable method if and only if at least one object is selected.
    /// In this method we can add more condition.
    /// </summary>
    /// <returns>False if no transform is selected. True otherwise.</returns>
    [MenuItem("GameObject/Interaction/Make Interactable", true)]
    private static bool ValidateMakeInteractable() {
        // Return false if no transform is selected.
        return Selection.activeTransform != null;
    }

    /// <summary>
    /// Makes the gameobject go an Interactable object.
    /// It adds the Interactable script and the OutlineEffect material in first position while conserving other materials.
    /// </summary>
    /// <param name="go">The modified gameobject.</param>
    internal static void MakeGameObjectInteractable(GameObject go) {

        Renderer rend = go.GetComponent<Renderer>();

        // Outline shader
        Material[] oldMaterials = rend.sharedMaterials;
        Material[] newMateriels = new Material[oldMaterials.Length + 1];

        newMateriels[0] = RetrieveOutlineEffectMaterial(); //To place the outline shader at the first place FOREVER

        for (int i = 0; i < oldMaterials.Length; i++) {
            newMateriels[i + 1] = oldMaterials[i];
        }

        rend.sharedMaterials = newMateriels;

        // Interactable script
        AddInteractableScript(go);
    }

    #endregion

    #region Create pre-defined objects

    /// <summary>
    /// Create an empty Interactable mesh.
    /// The gameobject needs to be completed in order to work.
    /// </summary>
    /// <param name="menuCommand"></param>
    [MenuItem("GameObject/Interaction/Mesh Interactable", false, 50)]
    public static void CreateMeshInteractableGameObject(MenuCommand menuCommand) {

        // Gameobject & name
        GameObject go = new GameObject("MeshInteractable");

        // Position
        MenuUtility.PlaceInFrontOfCamera(go);

        // Renderer
        MeshRenderer renderer = go.AddComponent<MeshRenderer>();

        // Outline shader
        renderer.material = RetrieveOutlineEffectMaterial(); //To place the outline shader at the first place FOREVER

        // Mesh filter
        go.AddComponent<MeshFilter>();

        // Mesh Collider
        go.AddComponent<MeshCollider>();

        // Interactable
        AddInteractableScript(go);

        // Scene utility
        GameObjectUtility.SetParentAndAlign(go, menuCommand.context as GameObject);

        Undo.RegisterCreatedObjectUndo(go, "Create " + go.name);
        Selection.activeObject = go;

    }


    /// <summary>
    /// Create a Cube primitive with Interactable script and utility.
    /// </summary>
    /// <param name="menuCommand"></param>
    [MenuItem("GameObject/Interaction/Cube Interactable", false, 100)]
    public static void CreateCubeInteractableGameObject(MenuCommand menuCommand) {

        // Gameobject & name
        GameObject go = GameObject.CreatePrimitive(PrimitiveType.Cube);
        go.name = "CubeInteractable";

        // Complete go with needed scripts
        CompletePrimitive(go);

        // Scene utility
        GameObjectUtility.SetParentAndAlign(go, menuCommand.context as GameObject);

        Undo.RegisterCreatedObjectUndo(go, "Create " + go.name);
        Selection.activeObject = go;

    }


    /// <summary>
    /// Create a Sphere primitive with Interactable script and utility.
    /// </summary>
    /// <param name="menuCommand"></param>
    [MenuItem("GameObject/Interaction/Sphere Interactable", false, 110)]
    public static void CreateSphereInteractableGameObject(MenuCommand menuCommand) {

        // Gameobject & name
        GameObject go = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        go.name = "SphereInteractable";

        // Complete go with needed scripts
        CompletePrimitive(go);

        // Scene utility
        GameObjectUtility.SetParentAndAlign(go, menuCommand.context as GameObject);

        Undo.RegisterCreatedObjectUndo(go, "Create " + go.name);
        Selection.activeObject = go;

    }


    /// <summary>
    /// Create a Capsule primitive with Interactable script and utility.
    /// </summary>
    /// <param name="menuCommand"></param>
    [MenuItem("GameObject/Interaction/Capsule Interactable", false, 120)]
    public static void CreateCapsuleInteractableGameObject(MenuCommand menuCommand) {

        // Gameobject & name
        GameObject go = GameObject.CreatePrimitive(PrimitiveType.Capsule);
        go.name = "CapsuleInteractable";

        // Complete go with needed scripts
        CompletePrimitive(go);

        // Scene utility
        GameObjectUtility.SetParentAndAlign(go, menuCommand.context as GameObject);

        Undo.RegisterCreatedObjectUndo(go, "Create " + go.name);
        Selection.activeObject = go;

    }


    /// <summary>
    /// Create a Cylinder primitive with Interactable script and utility.
    /// </summary>
    /// <param name="menuCommand"></param>
    [MenuItem("GameObject/Interaction/Cylinder Interactable", false, 130)]
    public static void CreateCylinderInteractableGameObject(MenuCommand menuCommand) {

        // Gameobject & name
        GameObject go = GameObject.CreatePrimitive(PrimitiveType.Cylinder);
        go.name = "CylinderInteractable";

        // Complete go with needed scripts
        CompletePrimitive(go);

        // Scene utility
        GameObjectUtility.SetParentAndAlign(go, menuCommand.context as GameObject);

        Undo.RegisterCreatedObjectUndo(go, "Create " + go.name);
        Selection.activeObject = go;

    }


    /// <summary>
    /// Create a Plane primitive with Interactable script and utility.
    /// </summary>
    /// <param name="menuCommand"></param>
    [MenuItem("GameObject/Interaction/Plane Interactable", false, 140)]
    public static void CreatePlaneInteractableGameObject(MenuCommand menuCommand) {

        // Gameobject & name
        GameObject go = GameObject.CreatePrimitive(PrimitiveType.Plane);
        go.name = "PlaneInteractable";

        // Complete go with needed scripts
        CompletePrimitive(go);

        // Scene utility
        GameObjectUtility.SetParentAndAlign(go, menuCommand.context as GameObject);

        Undo.RegisterCreatedObjectUndo(go, "Create " + go.name);
        Selection.activeObject = go;

    }

    #endregion

    #region Interactable Utility

    private static Material RetrieveOutlineEffectMaterial() {
        //return (Material)Resources.Load("Materials/OutlineEffect", typeof(Material));
        return (Material)AssetDatabase.LoadAssetAtPath("Assets/Materials/OutlineEffect.mat", typeof(Material));
    }

    /// <summary>
    /// Utility method used in each method creating a primitive Interactable.
    /// Calls the method adding the Interactable script.
    /// And adds the OutlineEffect material in first position.
    /// </summary>
    /// <param name="go">The modified gameobject.</param>
    private static void CompletePrimitive(GameObject go) {

        // Position
        MenuUtility.PlaceInFrontOfCamera(go);

        // Interactable
        AddInteractableScript(go);

        // Outline shader
        Renderer rend = go.GetComponent<Renderer>();

        Material[] ms = new Material[2];
        ms[0] = RetrieveOutlineEffectMaterial(); //To place the outline shader at the first place FOREVER
        ms[1] = rend.sharedMaterial;

        rend.sharedMaterials = ms;

    }


    /// <summary>
    /// Add the Interactable script and bind feedbacks to corresponding method.
    /// </summary>
    /// <param name="go">The modified gameobject.</param>
    private static void AddInteractableScript(GameObject go) {
        // Interactable script
        //Interactable interac = (Interactable)Undo.AddComponent(go, typeof(Interactable));
        Interactable interac = (Interactable)Undo.AddComponent(go, typeof(Interactable));

        // Show Interaction Feedback
        interac.onShowFeedback = new UnityEvent();
        UnityAction showMethodDelegate = System.Delegate.CreateDelegate(typeof(UnityAction), interac, "ShowOutlineEffect") as UnityAction;
        UnityEventTools.AddPersistentListener(interac.onShowFeedback, showMethodDelegate);

        // Hide Interaction Feedback
        interac.onHideFeedback = new UnityEvent();
        UnityAction hideMethodDelegate = System.Delegate.CreateDelegate(typeof(UnityAction), interac, "HideOutlineEffect") as UnityAction;
        UnityEventTools.AddPersistentListener(interac.onHideFeedback, hideMethodDelegate);
    }

    #endregion

}
