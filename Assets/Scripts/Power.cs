using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public abstract class Power<T> : MonoBehaviour where T : Power<T>
{
    public static T Instance;
    [SerializeField] protected CanvasGroup displayGroup;
    [SerializeField] protected Button backButton;
    [SerializeField] protected float cost;

    protected virtual void Awake()
    {
        Instance = (T)this;
    }


    public virtual void UsePower()
    {
        displayGroup.alpha = 1f;
        GameFlow.Instance.bottomGroup.gameObject.SetActive (false);
        GameFlow.Instance.topGroup.gameObject.SetActive (false);
        displayGroup.gameObject.SetActive (true);
        backButton.gameObject.SetActive(true);
    }

    public virtual void Back()
    {
        displayGroup.DOFade(0f, 0.2f).OnComplete(() =>
        {
            GameFlow.Instance.bottomGroup.gameObject.SetActive (true);
            GameFlow.Instance.topGroup.gameObject.SetActive (true);
            displayGroup.gameObject.SetActive (false);
        });  
        GameFlow.Instance.gameState = GameState.Playing;
    }
}
