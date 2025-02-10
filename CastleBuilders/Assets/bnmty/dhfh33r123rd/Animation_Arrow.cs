using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Animatiasdf : MonoBehaviour
{
    public float scaleMultiplier = 1.2f; 
    public float duration = 0.5f; 

    void Start()
    {
        StartPulse();
    }

    public void StartPulse()
    {
        Vector3 originalScale = transform.localScale;
        Vector3 targetScale = originalScale * scaleMultiplier;
        transform.DOScale(targetScale, duration)
            .SetLoops(-1, LoopType.Yoyo) 
            .SetEase(Ease.InOutSine); 
    }
}
