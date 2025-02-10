using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;

public class TutorialPanel : MonoBehaviour
{
    public Sprite[] images;
    public string[] titles;
    public string[] descriptions;

    public Image currentImage;
    public TextMeshProUGUI currentTitle;
    public TextMeshProUGUI currentDescription;

    public Button switchButton;
    public TextMeshProUGUI buttonText;

    private int currentIndex = 0;
    private float animationDuration = 0.5f;

    private void Start()
    {
        if (PlayerPrefs.GetInt("TutorialPanelShown", 0) == 1)
        {
            gameObject.SetActive(false);
        }

        UpdateContent(false);
    }

    public void SwitchContent()
    {
        currentIndex++;
        if (currentIndex >= images.Length)
        {
            currentIndex = 0;
            
            PlayerPrefs.SetInt("TutorialPanelShown", 1);
            PlayerPrefs.Save();

            gameObject.SetActive(false);
        }

        AnimatePageTurn();
    }

    private void AnimatePageTurn()
    {
        Sequence pageTurnSequence = DOTween.Sequence();

        pageTurnSequence.Append(currentImage.transform.DOScaleX(0, animationDuration / 2).SetEase(Ease.InQuad));
        pageTurnSequence.Join(currentTitle.transform.DOScaleX(0, animationDuration / 2).SetEase(Ease.InQuad));
        pageTurnSequence.Join(currentDescription.transform.DOScaleX(0, animationDuration / 2).SetEase(Ease.InQuad));

        pageTurnSequence.AppendCallback(() =>
        {
            UpdateContent(true);
        });

        pageTurnSequence.Append(currentImage.transform.DOScaleX(1, animationDuration / 2).SetEase(Ease.OutQuad));
        pageTurnSequence.Join(currentTitle.transform.DOScaleX(1, animationDuration / 2).SetEase(Ease.OutQuad));
        pageTurnSequence.Join(currentDescription.transform.DOScaleX(1, animationDuration / 2).SetEase(Ease.OutQuad));
    }

    private void UpdateContent(bool animate)
    {
        currentImage.sprite = images[currentIndex];
        currentTitle.text = titles[currentIndex];
        currentDescription.text = descriptions[currentIndex];

        if (currentIndex == 0)
        {
            buttonText.text = "Start Now";
        }
        else if (currentIndex == images.Length - 1)
        {
            buttonText.text = "Let's Start!";
        }
        else
        {
            buttonText.text = "Continue";
        }

        if (!animate)
        {
            currentImage.transform.localScale = Vector3.one;
            currentTitle.transform.localScale = Vector3.one;
            currentDescription.transform.localScale = Vector3.one;
        }
        else
        {
            Sequence fadeInSequence = DOTween.Sequence();

            currentImage.transform.localScale = new Vector3(0, 1, 1);
            currentTitle.transform.localScale = new Vector3(0, 1, 1);
            currentDescription.transform.localScale = new Vector3(0, 1, 1);

            fadeInSequence.Append(currentImage.transform.DOScaleX(1, animationDuration / 2).SetEase(Ease.OutQuad));
            fadeInSequence.Join(currentTitle.transform.DOScaleX(1, animationDuration / 2).SetEase(Ease.OutQuad));
            fadeInSequence.Join(currentDescription.transform.DOScaleX(1, animationDuration / 2).SetEase(Ease.OutQuad));
        }
    }
}
