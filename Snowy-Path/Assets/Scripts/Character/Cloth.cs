using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Character/Cloth")]
public class Cloth : ScriptableObject
{
    [System.Serializable]
    public enum EClothType {
        None,
        Light = 20,
        Normal = 35,
        Heavy = 75
    }

    public EClothType type;

    public float maxDurability = 1f;

    private float m_currentDurability = 0f;

    public void Init() {
        m_currentDurability = maxDurability;
    }
}
