using System.Collections.Generic;
using UnityEngine;
using FMODUnity;

[RequireComponent(typeof(StudioEventEmitter))]
public class Player_FootstepChoose : MonoBehaviour {

    [SerializeField] private float defaultParameterValue;
    [SerializeField] private LayerMask m_layers;

    [Space]
    [Header("Terrain layers")]
    [SerializeField] private SFXGroundType[] groundTypes;

    private StudioEventEmitter m_emitter;
    private float m_currentValue;
    private Terrain m_terrain;

    private FMOD.Studio.EventInstance instance;
    private FMOD.Studio.PARAMETER_ID paramId;

    private void Start() {
        m_emitter = GetComponent<StudioEventEmitter>();

        if (string.IsNullOrEmpty(m_emitter.Event)) {
            Debug.LogError("No event selected in " + gameObject.name);
            this.enabled = false;
        }
        else {
            InitializeParameter();
        }
    }

    private void Update() {
        ChooseSound();
    }

    private void InitializeParameter() {
        FMOD.Studio.EventDescription paramEventDesc = FMODUnity.RuntimeManager.GetEventDescription(m_emitter.Event);
        FMOD.Studio.PARAMETER_DESCRIPTION paramDesc;

        paramEventDesc.getParameterDescriptionCount(out int paramCount);

        if (paramCount > 0) {
            paramEventDesc.getParameterDescriptionByIndex(0, out paramDesc);
            paramId = paramDesc.id;
        }
    }

    private void SetParameter(float paramValue) {
        m_emitter.EventInstance.setParameterByID(paramId, paramValue);
    }

    /// <summary>
    /// Choose the sound value depending on the position the AI is standing on.
    /// </summary>
    public void ChooseSound() {

        RaycastHit _hitInfo;
        if (Physics.Raycast(transform.position, Vector3.down, out _hitInfo, 5.0f, m_layers)) {

            m_terrain = _hitInfo.transform.GetComponent<Terrain>();

            if (!m_terrain) {
                SetParameter(defaultParameterValue);
            }
            else {
                m_currentValue = ChooseSoundFromTexture(_hitInfo.point);
                SetParameter(m_currentValue);
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

        foreach (SFXGroundType ground in groundTypes) {

            if (ground.terrainLayers.Contains(m_terrain.terrainData.terrainLayers[activeTexture].name)) {
                return ground.value;
            }
        }

        return defaultParameterValue;
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

        for (int i = 0; i < numTextures; i++) {
            if (largestOpacity < splatmapData[(int)terrainCord.z, (int)terrainCord.x, i]) {
                activeTerrainIndex = i;
                largestOpacity = splatmapData[(int)terrainCord.z, (int)terrainCord.x, i];
            }
        }

        return activeTerrainIndex;
    }

}

[System.Serializable]
public class SFXGroundType {
    public List<string> terrainLayers;
    public float value;
}
