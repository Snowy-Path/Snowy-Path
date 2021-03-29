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
        m_text.text = "Cloth durability: " + inventory.GetCurrentCloth().GetCurrentDurability() + "/" + inventory.GetCurrentCloth().maxDurability;
    }
}
