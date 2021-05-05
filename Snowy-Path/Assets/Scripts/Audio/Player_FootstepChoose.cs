using System.Collections.Generic;
using UnityEngine;
using FMODUnity;

[RequireComponent(typeof(StudioEventEmitter))]
public class Player_FootstepChoose : MonoBehaviour {

    [SerializeField] private float terrainParameterValue;
    [SerializeField] private float rockParameterValue;
    [SerializeField] private LayerMask m_layers;

    private StudioEventEmitter m_emitter;

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

            var terrain = _hitInfo.transform.GetComponent<Terrain>();

            if (!terrain) {
                SetParameter(rockParameterValue);
            }
            else {
                SetParameter(terrainParameterValue);
            }
        }
    }
}