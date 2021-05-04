using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplorationMusic : MonoBehaviour {

    private static int m_nbWolfInCombat;
    private static bool m_isInCombat;

    public static void AddWolfInCombat() {
        m_nbWolfInCombat++;
        CheckPlayerInCombat();
    }

    public static void RemoveWolfInCombat() {
        m_nbWolfInCombat--;
        CheckPlayerInCombat();
    }

    private static void CheckPlayerInCombat() {
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

}
