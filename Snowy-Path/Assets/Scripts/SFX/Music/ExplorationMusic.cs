using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplorationMusic : MonoBehaviour {

    public static ExplorationMusic Instance { get; private set; }

    private int m_nbWolfInCombat;
    private bool m_isInCombat;

    private void Awake() {
        Instance = this;
    }

    public void AddWolfInCombat() {
        m_nbWolfInCombat++;
        CheckPlayerInCombat();
    }

    public void RemoveWolfInCombat() {
        m_nbWolfInCombat--;
        CheckPlayerInCombat();
    }

    private void CheckPlayerInCombat() {
        if (m_nbWolfInCombat > 0) {
            if (!m_isInCombat) {
                m_isInCombat = true;
                MusicManager.Instance.ChangeParametter("ExplorationAction", 1f);
            }
        } else {
            if (m_isInCombat) {
                m_isInCombat = false;
                MusicManager.Instance.ChangeParametter("ExplorationAction", 0f);
            }
        }
    }

    public void EnterSafeMusic() {
        MusicManager.Instance.ChangeParametter("ExplorationSafe", 1f);
    }

    public void ExitSafeMusic() {
        MusicManager.Instance.ChangeParametter("ExplorationSafe", 0f);
    }
}
