﻿using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.Events;
using UnityEngine;
using UnityEngine.Events;

public class InteractionMenu : MonoBehaviour
{

    /// <summary>
    /// Menu field. Makes all the selected gameobjects Interactable.
    /// </summary>
    /// <param name="menuCommand"></param>
    [MenuItem("GameObject/Interaction/MakeInteractable", false, 10)]
    static void MakeInteractable(MenuCommand menuCommand) {
        for (int i = 0; i < Selection.gameObjects.Length; i++) {
            MakeGameObjectInteractable(Selection.gameObjects[i]);
        }
    }

    /// <summary>
    /// Validation method to enable the previous MakeInteractable method if and only if at least one object is selected.
    /// In this method we can add more condition.
    /// </summary>
    /// <returns>False if no transform is selected. True otherwise.</returns>
    [MenuItem("GameObject/Interaction/MakeInteractable", true)]
    private static bool ValidateMakeInteractable() {
        // Return false if no transform is selected.
        return Selection.activeTransform != null;
    }

    /// <summary>
    /// Makes the gameobject go an Interactable object.
    /// It adds the Interactable script and the OutlineEffect material in first position while conserving other materials.
    /// </summary>
    /// <param name="go">The modified gameobject.</param>
    private static void MakeGameObjectInteractable(GameObject go) {

        // Outline shader
        Material[] oldMaterials = go.GetComponent<Renderer>().sharedMaterials;
        Material[] newMateriels = new Material[oldMaterials.Length + 1];

        newMateriels[0] = (Material)Resources.Load("Materials/OutlineEffect", typeof(Material)); //To place the outline shader at the first place FOREVER

        for (int i = 0; i < oldMaterials.Length; i++) {
            newMateriels[i + 1] = oldMaterials[i];
        }

        go.GetComponent<Renderer>().sharedMaterials = newMateriels;

        // Interactable script
        AddInteractableScript(go);
    }


    /// <summary>
    /// Create an empty Interactable mesh.
    /// The gameobject needs to be completed in order to work.
    /// </summary>
    /// <param name="menuCommand"></param>
    [MenuItem("GameObject/Interaction/MeshInteractable", false, 50)]
    static void CreateMeshInteractableGameObject(MenuCommand menuCommand) {

        // Gameobject & name
        GameObject go = new GameObject("MeshInteractable");

        // Position
        Camera sceneCam = SceneView.lastActiveSceneView.camera;
        Ray ray = new Ray(sceneCam.transform.position, sceneCam.transform.forward);

        RaycastHit info;
        if (Physics.Raycast(ray, out info, 50.0f)) {
            go.transform.position = info.point;
        } else {
            go.transform.position = sceneCam.transform.position + sceneCam.transform.forward * 50.0f;
        }

        // Renderer
        MeshRenderer renderer = go.AddComponent<MeshRenderer>();

        // Outline shader
        renderer.material = (Material)Resources.Load("Materials/OutlineEffect", typeof(Material)); //To place the outline shader at the first place FOREVER

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
    [MenuItem("GameObject/Interaction/CubeInteractable", false, 100)]
    static void CreateCubeInteractableGameObject(MenuCommand menuCommand) {

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
    [MenuItem("GameObject/Interaction/SphereInteractable", false, 110)]
    static void CreateSphereInteractableGameObject(MenuCommand menuCommand) {

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
    [MenuItem("GameObject/Interaction/CapsuleInteractable", false, 120)]
    static void CreateCapsuleInteractableGameObject(MenuCommand menuCommand) {

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
    [MenuItem("GameObject/Interaction/CylinderInteractable", false, 130)]
    static void CreateCylinderInteractableGameObject(MenuCommand menuCommand) {

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
    [MenuItem("GameObject/Interaction/PlaneInteractable", false, 140)]
    static void CreatePlaneInteractableGameObject(MenuCommand menuCommand) {

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


    /// <summary>
    /// Utility method used in each method creating a primitive Interactable.
    /// It moves the created gameobject in the center of the viewport like a simple Cube creation does.
    /// Calls the method adding the Interactable script.
    /// And adds the OutlineEffect material in first position.
    /// </summary>
    /// <param name="go">The modified gameobject.</param>
    private static void CompletePrimitive(GameObject go) {

        // Position
        Camera sceneCam = SceneView.lastActiveSceneView.camera;
        Ray ray = new Ray(sceneCam.transform.position, sceneCam.transform.forward);

        RaycastHit info;
        if (Physics.Raycast(ray, out info, 50.0f)) {
            go.transform.position = info.point;
        } else {
            go.transform.position = sceneCam.transform.position + sceneCam.transform.forward * 50.0f;
        }

        // Interactable
        AddInteractableScript(go);

        // Outline shader
        Material[] ms = new Material[2];
        ms[0] = (Material)Resources.Load("Materials/OutlineEffect", typeof(Material)); //To place the outline shader at the first place FOREVER
        ms[1] = go.GetComponent<Renderer>().sharedMaterial;

        go.GetComponent<Renderer>().sharedMaterials = ms;

    }


    /// <summary>
    /// Add the Interactable script and bind feedbacks to corresponding method.
    /// </summary>
    /// <param name="go">The modified gameobject.</param>
    private static void AddInteractableScript(GameObject go) {
        // Interactable script
        Interactable interac = (Interactable)go.AddComponent(typeof(Interactable));

        // Show Interaction Feedback
        interac.onShowFeedback = new UnityEvent();
        UnityAction showMethodDelegate = System.Delegate.CreateDelegate(typeof(UnityAction), interac, "ShowOutlineEffect") as UnityAction;
        UnityEventTools.AddPersistentListener(interac.onShowFeedback, showMethodDelegate);

        // Hide Interaction Feedback
        interac.onHideFeedback = new UnityEvent();
        UnityAction hideMethodDelegate = System.Delegate.CreateDelegate(typeof(UnityAction), interac, "HideOutlineEffect") as UnityAction;
        UnityEventTools.AddPersistentListener(interac.onHideFeedback, hideMethodDelegate);
    }

}
