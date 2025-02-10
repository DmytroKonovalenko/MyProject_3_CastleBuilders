using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CastleManager : MonoBehaviour
{
    public List<Castle> castles;

    public CastleManager()
    {
        castles = new List<Castle>();
        for (int i = 0; i < 15; i++) 
        {
           
        }     
    }  
    public void PurchaseCastle(int castleID)
    {
            SaveProgress(); 
    }
    public void PurchaseUpgrade(int castleID, int upgradeID)
    {

            castles[castleID].upgrades[upgradeID].isPurchased = true;
            SaveProgress(); 
    }

    public void SaveProgress()
    {
        string jsonData = JsonUtility.ToJson(this);
        System.IO.File.WriteAllText(Application.persistentDataPath + "/savefile.json", jsonData);
    }

    public void LoadProgress()
    {
        if (System.IO.File.Exists(Application.persistentDataPath + "/savefile.json"))
        {
            string jsonData = System.IO.File.ReadAllText(Application.persistentDataPath + "/savefile.json");
            JsonUtility.FromJsonOverwrite(jsonData, this);
        }
    }
}
