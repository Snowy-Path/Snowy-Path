using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "ReverbStrength", menuName = "ReverbStrength", order = 1)]
public class ReverbStrength : ScriptableObject {

    [Range(0, 1)]
    public float m_strength;

}
