using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class QBHeat_HPTempDisplay : MonoBehaviour
{
    public GameObject player;

    private Text m_text;

    void Start()
    {
        m_text = GetComponent<Text>();
    }

    void Update()
    {
        m_text.text = "HP: " + player.GetComponent<GenericHealth>().GetCurrentHealth() + " | Temp: " + player.GetComponent<Temperature>().GetCurrentTemperature();
    }
}
