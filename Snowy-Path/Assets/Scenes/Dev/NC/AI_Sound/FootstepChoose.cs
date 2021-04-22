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

    /// <summary>
    /// Choose the sound value depending on the position the AI is standing on.
    /// </summary>
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

    /// <summary>
    /// Detect if texture is in the list of snow sound.
    /// </summary>
    /// <param name="position"></param>
    /// <returns></returns>
    private float ChooseSoundFromTexture(Vector3 position) {

        int activeTexture = GetActiveTerrainTextureIdx(position);

        if (m_snowSoundLayers.Contains(m_terrain.terrainData.terrainLayers[activeTexture].name)) {
            return m_snowValue;
        }

        return m_rockValue;

    }

    /// <summary>
    /// Convert world position to Terrain position.
    /// </summary>
    /// <param name="worldPosition">World position to convert.</param>
    /// <returns>Position in Terrain texture.</returns>
    private Vector3 ConvertToSplatMapCoordinate(Vector3 worldPosition) {
        Vector3 splatPosition = new Vector3();
        Vector3 terPosition = m_terrain.transform.position;
        splatPosition.x = ((worldPosition.x - terPosition.x) / m_terrain.terrainData.size.x) * m_terrain.terrainData.alphamapWidth;
        splatPosition.z = ((worldPosition.z - terPosition.z) / m_terrain.terrainData.size.z) * m_terrain.terrainData.alphamapHeight;
        return splatPosition;
    }

    /// <summary>
    /// Return TerrainLayer ID depending on the Terrain texture position.
    /// </summary>
    /// <param name="position">Terrain texture position.</param>
    /// <returns>Terrain layer ID.</returns>
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

}
