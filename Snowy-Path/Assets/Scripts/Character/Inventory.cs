﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    private Cloth m_cloth;

    public void ChangeCloth(Cloth newCloth) {
        m_cloth = newCloth;
    }

    public Cloth GetCurrentCloth() {
        return m_cloth;
    }
}
