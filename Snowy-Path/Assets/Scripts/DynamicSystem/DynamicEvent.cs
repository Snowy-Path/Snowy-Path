using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Dynamic Event", fileName ="New Event")]
public class DynamicEvent : ScriptableObject {

    [SerializeField] new string name;
    public string Name { get => name; }
}
