using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public enum PowerType
{
    Hammer, Swap, Rocket, Paint
}

[Serializable]
public class PowerInfo
{
    public readonly float cost;
    public bool isTutorialFinish;

    public PowerInfo(float cost)
    {
        this.cost = cost;
        isTutorialFinish = false;
    }
}

public abstract class Power<T> : MonoBehaviour where T : Power<T>
{
    public static T Instance;
    [SerializeField] protected CanvasGroup displayGroup;
    [SerializeField] protected TextMeshProUGUI priceTxt;
    [SerializeField] protected Button backButton;
    [SerializeField] protected PowerType powerType;
    [SerializeField] protected PowerInfo info;
    public UnityEvent onUseCompleted;
    protected bool ignoreCost;
    protected float cost;
    public string ID { get; protected set; }
    protected virtual void Awake()
    {
        Instance = (T)this;
        info = GameSystem.userdata.dicPower[powerType];     
    }

    private void OnEnable()
    {
        DisplayCost();
    }

    public virtual void UsePower()
    {
        ignoreCost = false;
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

    public virtual void UsePowerIgnoreCost()
    {
        ignoreCost = true;
    }

    public void DisplayCost()
    {
        if (info.isTutorialFinish) cost = info.cost;
        else cost = 0;
        var text = cost > 0 ? cost.ToString() : "Free";
        priceTxt.text = text;
    }
    public void UpdateCost()
    {
        Debug.LogError(ignoreCost + "ignore???");
        if (!ignoreCost) GameFlow.Instance.diamondGroup.AddDiamond((int)-cost);
        GameFlow.Instance.diamondGroup.Display();
        GameSystem.SaveUserDataToLocal();
        DataUserManager.SaveUserData();
    }
}
