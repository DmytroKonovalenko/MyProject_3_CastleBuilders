using UnityEngine;
using TMPro;

public class UnlockNextCastleButton : MonoBehaviour
{
    [SerializeField] private GameObject specialButton;

    public LevelsPanel levelsPanel;

    private void Start()
    {
      

        if (levelsPanel == null)
        {
            Debug.LogError("LevelsPanel not found in the scene.");
            return;
        }
        UpdateButtonState();
        if (CoinsController.Instance != null)
        {
            CoinsController.Instance.OnCoinsChanged += UpdateButtonState;
        }
        else
        {
            Debug.LogError("CoinsController.Instance is null!");
        }
    }

    private void OnDestroy()
    {
        if (CoinsController.Instance != null)
        {
            CoinsController.Instance.OnCoinsChanged -= UpdateButtonState;
        }
    }

    private void UpdateButtonState()
    {
        if (levelsPanel == null)
        {
            Debug.LogError("LevelsPanel is not initialized.");
            return;
        }

        int lastPurchasedCastleIndex = levelsPanel.GetLastPurchasedCastleIndex();
        Debug.Log($"Last Purchased Castle Index: {lastPurchasedCastleIndex}");

        int nextCastlePrice = levelsPanel.GetCastlePrice(lastPurchasedCastleIndex + 1);
        int playerCoins = PlayerPrefs.GetInt("Coins", 0); ;

        Debug.Log($"Next Castle Price: {nextCastlePrice}");
        Debug.Log($"Player Coins: {playerCoins}");

        if (lastPurchasedCastleIndex >= 0 && playerCoins >= nextCastlePrice)
        {
            specialButton.SetActive(true);
        }
        else
        {
            specialButton.SetActive(false);
        }
    }
}
