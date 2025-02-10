using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Upgrade
{
    public int upgradeID; 
    public bool isPurchased; 

    public Upgrade(int id)
    {
        upgradeID = id;
        isPurchased = false;
    }
}