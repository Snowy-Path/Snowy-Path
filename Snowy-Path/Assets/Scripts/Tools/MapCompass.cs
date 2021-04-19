using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class MapCompass : MonoBehaviour, IHandTool {
    public EToolType ToolType => EToolType.MapCompass;
    public bool IsBusy { get; set; }

    //private MapUI m_mapUI;

    private PlayerInput m_playerInput;
    private Camera m_inGameMapCamera;

    public Camera InGameMapCamera { get { return m_inGameMapCamera; } }

    void Start()
    {
        //m_mapUI = Utils.FindComponent<MapUI>("MapCanvas/Map");

        //m_inGameMapCamera = GetComponentInChildren<Camera>();
        m_playerInput = GetComponentInParent<PlayerInput>();
    }

    public void SwitchCurrentActionMap(string name)
    {
        //m_playerInput.SwitchCurrentActionMap(name);
    }

    public void StartPrimaryUse()
    {
        //m_mapUI.OpenFullscreenMap();
    }

    public void CancelPrimaryUse()
    {
    }

    public void SecondaryUse()
    {
        //m_mapUI.OpenFullscreenMap();
    }

    public void ToggleDisplay(bool display) {
        gameObject.SetActive(display);
    }
}
