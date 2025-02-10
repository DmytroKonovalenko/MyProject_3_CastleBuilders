using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class UpgradePopup : MonoBehaviour
{
    [SerializeField] private Castle[] castles;
    [SerializeField] private TextMeshProUGUI[] upgradeNames;
    [SerializeField] private TextMeshProUGUI[] upgradePrices;
    [SerializeField] private Button[] purchaseButtons;
    [SerializeField] private Image[] purchaseButtonImages;
    [SerializeField] private Sprite activeButtonSprite;
    [SerializeField] private Sprite inactiveButtonSprite;
    [SerializeField] private Sprite noMoneyButtonSprite;
    [SerializeField] private GameObject[] upgradePurchasedObjects;
    [SerializeField] private GameObject[] upgradeNotPurchasedObjects;
    [SerializeField] private TextMeshProUGUI upgradesCounterText;
    [SerializeField] private TextMeshProUGUI upgradesCounterText2;

    private int selectedCastleIndex = 0;
    private const string UpgradeKeyPrefix = "CastleUpgrade_";

    private void Start()
    {
        LoadPurchasedUpgrades();
        DisplayUpgrades();
    }

    private void DisplayUpgrades()
    {
        Castle selectedCastle = castles[selectedCastleIndex];
        int playerCoins = CoinsController.Instance.GetCoins();

        for (int i = 0; i < selectedCastle.upgrades.Length; i++)
        {
            upgradeNames[i].text = selectedCastle.upgrades[i].upgradeName;
            upgradePrices[i].text = selectedCastle.upgrades[i].isPurchased ? "Purchased" : $"{selectedCastle.upgrades[i].upgradePrice}";

            if (!selectedCastle.upgrades[i].isPurchased && playerCoins < selectedCastle.upgrades[i].upgradePrice)
            {
                purchaseButtons[i].interactable = false;
                purchaseButtonImages[i].sprite = noMoneyButtonSprite;
            }
            else
            {
                UpdatePurchaseButton(i, selectedCastle.upgrades[i].isPurchased);
            }
            if (selectedCastle.upgrades[i].isPurchased)
            {
                upgradePurchasedObjects[i].SetActive(true);
                upgradeNotPurchasedObjects[i].SetActive(false);
            }
            else
            {
                upgradePurchasedObjects[i].SetActive(false);
                upgradeNotPurchasedObjects[i].SetActive(true);
            }
        }

        UpdateUpgradesCounter();
    }


    private void UpdatePurchaseButton(int index, bool isPurchased)
    {
        if (isPurchased)
        {
            purchaseButtons[index].interactable = false;
            purchaseButtonImages[index].sprite = inactiveButtonSprite;
            upgradePurchasedObjects[index].SetActive(true);
            upgradeNotPurchasedObjects[index].SetActive(false);
        }
        else
        {
            purchaseButtons[index].interactable = true;
            purchaseButtonImages[index].sprite = activeButtonSprite;
            upgradePurchasedObjects[index].SetActive(false);
            upgradeNotPurchasedObjects[index].SetActive(true);
        }
    }

    public void PurchaseUpgrade(int upgradeIndex)
    {
        Castle selectedCastle = castles[selectedCastleIndex];
        CastleUpgrade upgrade = selectedCastle.upgrades[upgradeIndex];

        int playerCoins = CoinsController.Instance.GetCoins();

        if (!upgrade.isPurchased && playerCoins >= upgrade.upgradePrice)
        {
            CoinsController.Instance.SubtractCoins(upgrade.upgradePrice);
            upgrade.isPurchased = true;
            SavePurchasedUpgrade(selectedCastleIndex, upgradeIndex);
            DisplayUpgrades();
        }
        else
        {
            Debug.Log("Недостатньо монет або покращення вже куплено!");
        }
    }

    private void SavePurchasedUpgrade(int castleIndex, int upgradeIndex)
    {
        PlayerPrefs.SetInt(UpgradeKeyPrefix + castleIndex + "_" + upgradeIndex, 1);
        PlayerPrefs.Save();
    }

    public void LoadPurchasedUpgrades()
    {
        for (int i = 0; i < castles.Length; i++)
        {
            for (int j = 0; j < castles[i].upgrades.Length; j++)
            {
                castles[i].upgrades[j].isPurchased = PlayerPrefs.GetInt(UpgradeKeyPrefix + i + "_" + j, 0) == 1;
            }
        }
    }

    public void SelectCastle(int newIndex)
    {
        selectedCastleIndex = newIndex;
        DisplayUpgrades();
    }

    public void UpdateUpgradesCounter()
    {
        Castle selectedCastle = castles[selectedCastleIndex];
        int purchasedUpgradesCount = 0;

        foreach (var upgrade in selectedCastle.upgrades)
        {
            if (upgrade.isPurchased)
            {
                purchasedUpgradesCount++;
            }
        }

        upgradesCounterText.text = $"{purchasedUpgradesCount}/{selectedCastle.upgrades.Length}";
        upgradesCounterText2.text = $"{purchasedUpgradesCount}/{selectedCastle.upgrades.Length}";
    }
}
