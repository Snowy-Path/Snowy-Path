using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FootstepChoose : MonoBehaviour {

    #region Variables

    [SerializeField]
    private LayerMask m_layers;

    [SerializeField]
    private FmodAudioPlayer m_emitter;

    [Header("Sound variance")]
    [SerializeField]
    [Tooltip("FMOD value for a snow sound")]
    private float m_snowValue;

    [SerializeField]
    [Tooltip("FMOD value for a rock sound. Represent the default value.")]
    private float m_rockValue;

    [SerializeField]
    private List<string> m_snowSoundLayers;

    private float m_currentValue;

    #endregion

    private Terrain m_terrain;

    public void ChooseSound() {

        RaycastHit _hitInfo;
        if (Physics.Raycast(transform.position, Vector3.down, out _hitInfo, 5.0f, m_layers)) {

            m_terrain = _hitInfo.transform.GetComponent<Terrain>();

            if (!m_terrain) {
                m_emitter.SetParam(m_rockValue);
            } else {
                m_currentValue = ChooseSoundFromTexture(_hitInfo.point);
                m_emitter.SetParam(m_currentValue);
            }

        }

    }

    private float ChooseSoundFromTexture(Vector3 position) {

        int activeTexture = GetActiveTerrainTextureIdx(position);

        Debug.Log(activeTexture);

        if (m_snowSoundLayers.Contains(m_terrain.terrainData.terrainLayers[activeTexture].name)) {
            return m_snowValue;
        }

        return m_rockValue;

    }

    private Vector3 ConvertToSplatMapCoordinate(Vector3 worldPosition) {
        Vector3 splatPosition = new Vector3();
        Vector3 terPosition = m_terrain.transform.position;
        splatPosition.x = ((worldPosition.x - terPosition.x) / m_terrain.terrainData.size.x) * m_terrain.terrainData.alphamapWidth;
        splatPosition.z = ((worldPosition.z - terPosition.z) / m_terrain.terrainData.size.z) * m_terrain.terrainData.alphamapHeight;
        return splatPosition;
    }

    private int GetActiveTerrainTextureIdx(Vector3 position) {
        Vector3 terrainCord = ConvertToSplatMapCoordinate(position);
        int activeTerrainIndex = 0;
        float largestOpacity = 0f;

        TerrainData terrainData = m_terrain.terrainData;

        int alphamapWidth = terrainData.alphamapWidth;
        int alphamapHeight = terrainData.alphamapHeight;

        float[,,] splatmapData = terrainData.GetAlphamaps(0, 0, alphamapWidth, alphamapHeight);
        int numTextures = splatmapData.Length / (alphamapWidth * alphamapHeight);

        for (int i = 0; i<numTextures; i++) {
            if (largestOpacity<splatmapData[(int)terrainCord.z, (int)terrainCord.x, i]) {
                activeTerrainIndex = i;
                largestOpacity = splatmapData[(int)terrainCord.z, (int)terrainCord.x, i];
            }
        }

        return activeTerrainIndex;
    }

    //private void OnGUI() {

    //    GUI.Box(new Rect(Screen.width - 110, 10, 100, 70), "Sound value");
    //    GUI.Label(new Rect(Screen.width - 100, 40, 90, 70), $"{m_currentValue}");

    //}

}
