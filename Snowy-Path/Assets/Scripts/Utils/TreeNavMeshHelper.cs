using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class TreeNavMeshHelper : MonoBehaviour {
    public GameObject navMeshModifier;
    private Terrain terrain;

    public void Generate() {
        terrain = GetComponent<Terrain>();
        if (!terrain)
            return;

        GameObject folder = GameObject.Find("TreeNavMeshes");
        Vector3 size = terrain.terrainData.size;
        TreeInstance instance;
        if (!folder)
            folder = new GameObject("TreeNavMeshes");

        for (int i = 0; i < terrain.terrainData.treeInstanceCount; i++) {
            instance = terrain.terrainData.treeInstances[i];
            Vector3 pos = new Vector3(
                instance.position.x * size.x + terrain.transform.position.x,
                instance.position.y * size.y + terrain.transform.position.y,
                instance.position.z * size.z + terrain.transform.position.z);

            Instantiate(navMeshModifier, pos, Quaternion.Euler(0, instance.rotation, 0), folder.transform);
        }
    }

    public void Clear() {
        GameObject folder = GameObject.Find("TreeNavMeshes");
        if (folder)
            DestroyImmediate(folder);
    }
}
