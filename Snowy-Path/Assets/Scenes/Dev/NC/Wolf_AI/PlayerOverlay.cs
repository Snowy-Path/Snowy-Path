﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerOverlay : MonoBehaviour {

    public Inventory inventory;
    public GenericHealth playerHealth;
    public Temperature temperature;

    public Cloth cloth;

    //private void Start() {
    //    cloth.Init();
    //    inventory.ChangeCloth(cloth);
    //}

    private void OnGUI() {

        GUI.Box(new Rect(10, 10, 200, 130), "Player's data");

        Cloth hasCloth = inventory.GetCurrentCloth();
        if (hasCloth) {
            GUI.Label(new Rect(20, 40, 180, 30), $"Cloth dur : {hasCloth.GetCurrentDurability()}");
        }
        GUI.Label(new Rect(20, 70, 180, 30), $"Player HP : {playerHealth.GetCurrentHealth()}");
        GUI.Label(new Rect(20, 100, 180, 30), $"Player °C : {temperature.GetCurrentTemperature()}");

    }
}
