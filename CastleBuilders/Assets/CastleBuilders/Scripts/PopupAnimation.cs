using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PopupAnimation : MonoBehaviour
{

    [SerializeField] private RectTransform panelRectTransform;
    [SerializeField] private float animationDuration = 0.5f;

    private void OnEnable()
    {
        AnimateOpenPanel();
    }

   

    private void AnimateOpenPanel()
    {
        panelRectTransform.localScale = Vector3.zero;

        Sequence animationSequence = DOTween.Sequence();
        animationSequence.Append(panelRectTransform.DOScale(Vector3.one, animationDuration).SetEase(Ease.OutBack));
    }

    public void AnimateClosePanel()
    {
        Sequence animationSequence = DOTween.Sequence();
        animationSequence.Append(panelRectTransform.DOScale(Vector3.zero, animationDuration).SetEase(Ease.InBack));
        animationSequence.OnComplete(() => gameObject.SetActive(false));
    }
}


