using UnityEngine;
using TMPro;
using System.Collections.Generic;

public class CoinsController : MonoBehaviour
{
    public static CoinsController Instance { get; private set; }
    private BottomPanelManager uiManager;
    private int coins;
    
    public event System.Action OnCoinsChanged;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {   
        LoadCoins();   
        FindUI();
    }

    public void AddCoins(int amount)
    {
        coins += amount;
        SaveCoins();     
        OnCoinsChanged?.Invoke();
        FindUI();
    }

    public void SubtractCoins(int amount)
    {
        if (coins >= amount)
        {
            coins -= amount;
            SaveCoins();        
            OnCoinsChanged?.Invoke();
            FindUI();
        }
        else
        {
            Debug.Log("Недостатньо монет для транзакції!");
        }
    }

    private void SaveCoins()
    {
        PlayerPrefs.SetInt("Coins", coins);
        PlayerPrefs.Save();
    }

    public void LoadCoins()
    {
        coins = PlayerPrefs.GetInt("Coins", 0);
    }

    private void UpdateCoinsUI()
    {

        if (uiManager == null)
        {
            uiManager = FindObjectOfType<BottomPanelManager>();
            if (uiManager == null) return;
        }

        foreach (var text in uiManager.text)
        {
            if (text) text.text = coins.ToString();
        }

       
    }

    public int GetCoins()
    {
        return coins;
    }
    public void FindUI()
    {
        uiManager = FindObjectOfType<BottomPanelManager>();
        UpdateCoinsUI();
    }
}
