using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using TMPro;

public class LevelsPanel : MonoBehaviour
{
  //  [SerializeField] private RectTransform panelRectTransform;
    [SerializeField] private float animationDuration = 0.5f;

    [SerializeField] private Image castleImage;
    [SerializeField] private Image lockImage;
    [SerializeField] private TextMeshProUGUI castleTitle;
    [SerializeField] private TextMeshProUGUI castleDescription;
    [SerializeField] private TextMeshProUGUI castleLevel;
    [SerializeField] private TextMeshProUGUI castlePrice;
    [SerializeField] private TextMeshProUGUI upgradeCountText;
    [SerializeField] private Button purchaseButton;
    [SerializeField] private Image purchaseButtonImage;
    [SerializeField] private TextMeshProUGUI purchaseButtonText;
    [SerializeField] private Sprite activeButtonSprite;
    [SerializeField] private Sprite inactiveButtonSprite;
    [SerializeField] private Sprite[] castleImages;
    [SerializeField] private string[] castleTitles;
    [SerializeField] private string[] castleDescriptions;
    [SerializeField] private string[] castleLevels;
    [SerializeField] private int[] castlePrices;

    public StartPanel startPanel;
    public UpgradePopup upgradePopup;

    private bool[] castlesPurchased;
    private int currentIndex = 0;

    private const string CastleKeyPrefix = "CastlePurchased_";

    private int[] purchasedUpgradesCount;
    private const int maxUpgradesPerCastle = 5; 

    private void Start()
    {
        
        castlesPurchased = new bool[castleImages.Length];
        purchasedUpgradesCount = new int[castleImages.Length]; 

        LoadPurchasedCastles();
        LoadPurchasedUpgrades();  
        upgradePopup.LoadPurchasedUpgrades();
        upgradePopup.UpdateUpgradesCounter();
        castlesPurchased[0] = true;
        ActivateLastPurchasedCastle();


    }

    private void OnEnable()
    {
        upgradePopup.LoadPurchasedUpgrades();
        upgradePopup.UpdateUpgradesCounter();
        ActivateLastPurchasedCastle();
        // UpdateLevelInfo();



    }

    

    public void UpdateLevelInfo()
    {
        castleTitle.text = castleTitles[currentIndex];
        castleDescription.text = castleDescriptions[currentIndex];
        castleLevel.text = castleLevels[currentIndex];
        castlePrice.text = $"{castlePrices[currentIndex]}";

        UpdatePurchaseButton();
        castleImage.sprite = castleImages[currentIndex];

        if (castlesPurchased[currentIndex])
        {
            castleImage.color = Color.white;
            lockImage.gameObject.SetActive(false);
        }
        else
        {
            castleImage.color = Color.black;
            lockImage.gameObject.SetActive(true);
        }
        UpdateUpgradeCountText();
        upgradePopup.SelectCastle(currentIndex);
    }

    private void UpdateUpgradeCountText()
    {
        upgradeCountText.text = $"{purchasedUpgradesCount[currentIndex]}/{maxUpgradesPerCastle}";
    }

    public void NextLevel()
    {
        currentIndex = (currentIndex + 1) % castleImages.Length;
        AnimateTransition();
    }

    public void PreviousLevel()
    {
        currentIndex = (currentIndex - 1 + castleImages.Length) % castleImages.Length;
        AnimateTransition();
    }

    private void AnimateTransition()
    {
        Sequence transitionSequence = DOTween.Sequence();
        transitionSequence.Append(castleImage.DOFade(0f, animationDuration / 2).SetEase(Ease.InOutQuad))
                          .OnComplete(() =>
                          {
                              UpdateLevelInfo();
                              castleImage.color = new Color(castleImage.color.r, castleImage.color.g, castleImage.color.b, 0);
                              castleImage.DOFade(1f, animationDuration / 2).SetEase(Ease.InOutQuad);
                          });
    }

    public void PurchaseCastle()
    {
        int playerCoins = CoinsController.Instance.GetCoins();

        if (playerCoins >= castlePrices[currentIndex])
        {

            if (!castlesPurchased[currentIndex]) 
            {
      
                CoinsController.Instance.SubtractCoins(castlePrices[currentIndex]);
                castlesPurchased[currentIndex] = true;
                SavePurchasedCastle(currentIndex);
                UpdateLevelInfo();
                startPanel.UpdateLastPurchasedCastleUI();
            }
            else
            {
                Debug.Log("Замок вже куплений!");
            }
        }
        else
        {
            Debug.Log("Недостатньо монет!");
        }
    }


    private void UpdatePurchaseButton()
    {
        int playerCoins = CoinsController.Instance.GetCoins();

        if (castlesPurchased[currentIndex])
        {
            SetPurchaseButtonState(false, inactiveButtonSprite, "PURCHASED");
        }
        else if (currentIndex > 0 && !castlesPurchased[currentIndex - 1] || playerCoins < castlePrices[currentIndex])
        {
            SetPurchaseButtonState(false, inactiveButtonSprite, "LOCKED");
        }
        else
        {
            SetPurchaseButtonState(true, activeButtonSprite, "BUY");
        }
    }

    private void SetPurchaseButtonState(bool isAvailable, Sprite buttonSprite, string buttonText)
    {
        purchaseButton.interactable = isAvailable;
        purchaseButtonImage.sprite = buttonSprite;
        purchaseButtonText.text = buttonText;
    }

    private void SavePurchasedCastle(int index)
    {
        PlayerPrefs.SetInt(CastleKeyPrefix + index, 1);
        PlayerPrefs.Save();
    }

    private void LoadPurchasedCastles()
    {
        for (int i = 0; i < castleImages.Length; i++)
        {
            castlesPurchased[i] = PlayerPrefs.GetInt(CastleKeyPrefix + i, 0) == 1;
        }
    }


    private void SavePurchasedUpgrades()
    {
        for (int i = 0; i < castleImages.Length; i++)
        {
            PlayerPrefs.SetInt($"CastleUpgrades_{i}", purchasedUpgradesCount[i]);
        }
        PlayerPrefs.Save();
    }


    private void LoadPurchasedUpgrades()
    {
        for (int i = 0; i < castleImages.Length; i++)
        {
            purchasedUpgradesCount[i] = PlayerPrefs.GetInt($"CastleUpgrades_{i}", 0);
        }
    }
    public int GetCurrentIndex()
    {
        return currentIndex;
    }

    public int GetTotalCastles()
    {
        return castleImages.Length;
    }

    public int GetCastlePrice(int index)
    {
        return castlePrices[index];
    }
    public int GetLastPurchasedCastleIndex()
    {
        int lastPurchasedIndex = -1;
        int maxCastleIndex = castlePrices.Length; 

        for (int i = 0; i < maxCastleIndex; i++)
        {
            int status = PlayerPrefs.GetInt($"CastlePurchased_{i}", 0);
            if (status == 1)
            {
                lastPurchasedIndex = i;
            }
        }

        return lastPurchasedIndex;
    }

 
    public bool IsCastlePurchased(int index)
    {
        return PlayerPrefs.GetInt($"CastlePurchased_{index}", 0) == 1;
    }
    public void ActivateLastPurchasedCastle()
    {
   
        int lastPurchasedIndex = GetLastPurchasedCastleIndex();

        if (lastPurchasedIndex != -1)
        {
     
            currentIndex = lastPurchasedIndex;

            UpdateLevelInfo();

    
            castleImage.sprite = castleImages[currentIndex];


            castleImage.color = Color.white;
            lockImage.gameObject.SetActive(false);
        }
        else
        {
            Debug.Log("Немає куплених замків для активації.");
        }
    }

}
