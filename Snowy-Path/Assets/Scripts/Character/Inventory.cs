using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour {
    public Cloth defaultCloth;

    private Cloth m_cloth;

    void Start()
    {
        if (defaultCloth == null)
            Debug.LogError("[Inventory] Default cloth is not set.");

        m_cloth = defaultCloth;
    }

    public void ChangeCloth(Cloth newCloth) {
        m_cloth = newCloth;
    }

    public Cloth GetCurrentCloth() {
        return m_cloth;
    }

    /// <summary>
    /// Call the method to reduce the durability on the current cloth.
    /// </summary>
    /// <param name="percentage">Percentage to deduce to the current durability.</param>
    public void ReduceClothDurabilityPercentage(float percentage) {
        if (m_cloth) {
            m_cloth.ReduceDurabilityPercentage(percentage);
        }
    }
}
