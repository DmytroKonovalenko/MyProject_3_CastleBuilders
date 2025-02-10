using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using UnityEngine.SceneManagement;
using TMPro;

public class BottomPanelManager : MonoBehaviour
{
    [System.Serializable]
    public class PanelData
    {
        public GameObject panel;
        public Button button;
        public Image buttonImage;
        public Sprite inactiveSprite;
        public Sprite activeSprite;
        public GameObject indicator;
    }

    [SerializeField] private List<PanelData> panels;
    [SerializeField] private float animationDuration = 0.5f;
    [SerializeField] private float slideDistance = 1500f;

    private PanelData currentPanel;

    public List<TextMeshProUGUI> text;

    private void Start()
    {
        CoinsController.Instance.FindUI();
 DOTween.Init();   
        foreach (var panelData in panels)
        {
            panelData.button.onClick.AddListener(() => SwitchPanel(panelData));
            panelData.panel.SetActive(false);
        }

        SwitchPanel(panels[0], true);
    }
    public void OpenUpgradesPanel()
    {
        SwitchPanel(panels[2], false);
    }
    public void OpenGamePanel()
    {
        SwitchPanel(panels[1], false);
    }
    private void SwitchPanel(PanelData newPanel, bool instant = false)
    {
        if (currentPanel == newPanel) return;

        PanelData previousPanel = currentPanel;
        currentPanel = newPanel;

        if (previousPanel != null)
        {
            bool newPanelRight = newPanel.panel.transform.GetSiblingIndex() > previousPanel.panel.transform.GetSiblingIndex();
            Vector3 exitPosition = newPanelRight ? new Vector3(-slideDistance, 0, 0) : new Vector3(slideDistance, 0, 0);

            previousPanel.panel.transform.DOLocalMove(exitPosition, animationDuration).SetEase(Ease.InOutQuad);
        }

        newPanel.panel.SetActive(true);
        bool newPanelLeft = previousPanel != null && newPanel.panel.transform.GetSiblingIndex() < previousPanel.panel.transform.GetSiblingIndex();
        Vector3 startPosition = newPanelLeft ? new Vector3(-slideDistance, 0, 0) : new Vector3(slideDistance, 0, 0);

        newPanel.panel.transform.localPosition = startPosition;
        newPanel.panel.transform.DOLocalMove(Vector3.zero, animationDuration).SetEase(Ease.OutQuad)
            .OnComplete(() =>
            {
                if (previousPanel != null)
                {
                    previousPanel.panel.SetActive(false);
                }
            });

        foreach (var panelData in panels)
        {
            bool isActive = panelData == newPanel;
            panelData.buttonImage.sprite = isActive ? panelData.activeSprite : panelData.inactiveSprite;
            panelData.indicator.SetActive(isActive);
        }
    }
    public void LoadGame()
    {
        SceneManager.LoadScene(1);
    }
}
