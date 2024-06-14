using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DarkcupGames;
using DG.Tweening;

[RequireComponent(typeof(CanvasGroup))]
public class FadeInOut : MonoBehaviour
{
    public const float FADE_SPEED = 1f;

    public CanvasGroup canvasGroup { get; private set; }

    private void Awake()
    {
        canvasGroup = GetComponent<CanvasGroup>();
    }
    public void FadeOut()
    {
        canvasGroup.alpha = 1f;
        canvasGroup.DOFade(0f, FADE_SPEED).OnComplete(() =>
        {
            gameObject.SetActive(false);
        });
    }
    public void FadeIn(System.Action onDone)
    {
        gameObject.SetActive(true);
        canvasGroup.alpha = 0f;
        canvasGroup.DOFade(1f, FADE_SPEED).OnComplete(() =>
        {
            onDone?.Invoke();
        });
    }
}