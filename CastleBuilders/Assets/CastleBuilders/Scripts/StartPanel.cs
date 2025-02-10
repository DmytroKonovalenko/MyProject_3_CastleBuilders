using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class StartPanel : MonoBehaviour
{
    [SerializeField] private Image lastCastleImage;
    [SerializeField] private TextMeshProUGUI lastCastleTitle;
    [SerializeField] private TextMeshProUGUI lastCastleLevel; 

    [SerializeField] private Sprite[] castleImages; 
    [SerializeField] private string[] castleTitles; 
    [SerializeField] private string[] castleLevels; 

    private bool[] castlesPurchased; 

    private void OnEnable()
    {
        LoadPurchasedCastles();
        UpdateLastPurchasedCastleUI();
    }
    private void Start()
    {
        LoadPurchasedCastles();
        UpdateLastPurchasedCastleUI();
    }

    private void LoadPurchasedCastles()
    {
        castlesPurchased = new bool[castleImages.Length];
        for (int i = 0; i < castlesPurchased.Length; i++)
        {
            castlesPurchased[i] = PlayerPrefs.GetInt($"CastlePurchased_{i}", 0) == 1;
        }
    }

    public void UpdateLastPurchasedCastleUI()
    {
        int lastPurchasedIndex = GetLastPurchasedCastleIndex();

        if (lastPurchasedIndex >= 0)
        {
            lastCastleImage.sprite = castleImages[lastPurchasedIndex];
            lastCastleTitle.text = castleTitles[lastPurchasedIndex];
            lastCastleLevel.text = castleLevels[lastPurchasedIndex];
        }
        else
        {
            Debug.Log("Немає куплених замків.");
        }
    }

    private int GetLastPurchasedCastleIndex()
    {
        for (int i = castlesPurchased.Length - 1; i >= 0; i--)
        {
            if (castlesPurchased[i])
            {
                return i;
            }
        }
        return -1;
    }
}
