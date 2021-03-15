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

    /// <summary>
    /// Reduce the current durability from a given percentage.
    /// Primarly used when the player is attacked by an ennemy (Wolf for example).
    /// </summary>
    /// <param name="percentage">Percentage to deduce to the current durability</param>
    public void ReduceDurabilityPercentage(float percentage) {
        m_currentDurability -= m_currentDurability * percentage;
    }

    public float GetCurrentDurability() {
        return m_currentDurability;
    }
}
