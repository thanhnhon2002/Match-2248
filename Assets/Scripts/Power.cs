using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Power : MonoBehaviour
{
    [SerializeField] protected CanvasGroup displayGroup;

    public virtual void UsePower()
    {
        displayGroup.alpha = 1f;
        GameFlow.Instance.bottomGroup.SetActive (false);
        displayGroup.gameObject.SetActive (true);
    }

    public virtual void Back()
    {
        displayGroup.DOFade(0f, 0.2f).OnComplete(() => displayGroup.gameObject.SetActive (false));  
        GameFlow.Instance.gameState = GameState.Playing;
    }
}
