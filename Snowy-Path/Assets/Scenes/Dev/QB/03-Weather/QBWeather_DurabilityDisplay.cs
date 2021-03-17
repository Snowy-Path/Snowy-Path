using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class QBWeather_DurabilityDisplay : MonoBehaviour
{
    public Inventory inventory;

    private Text m_text;

    void Start()
    {
        m_text = GetComponent<Text>();
    }

    void Update()
    {
        var cloth = inventory.GetCurrentCloth();
        if (cloth != null) {
            var durability = cloth.GetCurrentDurability();
            var maxDurability = cloth.maxDurability;
            m_text.text = "Cloth durability: " + maxDurability + "/" + maxDurability;
        }
    }
}
