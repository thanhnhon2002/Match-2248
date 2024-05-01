using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DarkcupGames;
using DG.Tweening;

[Serializable]
public enum PopupOptions
{
    NewBlock,
    BlockAdded,
    LockElinimated,
    RateGame,
    Pause,
    MakeSure,
    Lose,
    Duplicate,
    StartFrom
}
public class DataEventPopup : EventArgs
{
    public Action<PopupOptions> action;
    public PopupOptions option;
    public DataEventPopup(Action<PopupOptions> action, PopupOptions option)
    {
        this.action = action;
        this.option = option;
    }
}

public class PopupManager : MonoBehaviour
{
    public static PopupManager Instance;
    public Popup[] popups;
    private Dictionary<PopupOptions, Popup> popupDic = new Dictionary<PopupOptions, Popup>();
    public Image blackBackground;
    private Queue<DataEventPopup>queueShow=new Queue<DataEventPopup>();
    private void Start()
    {
        SubShowPopup(new DataEventPopup(PopupManager.Instance.ShowPopup, PopupOptions.StartFrom));
    }
    private void Awake()
    {
        Instance = this;
        popups = GetComponentsInChildren<Popup>(true);
        foreach (var item in popups)
        {
            popupDic.Add(item.option, item);
        }
    }
    public void SubShowPopup(DataEventPopup data)
    {
        queueShow.Enqueue(data);
        if (queueShow.Count == 1)
        {
            DOVirtual.DelayedCall(0.5f, () => this.ShowAllQueue());       
        }
    }
    public void ShowAllQueue()
    {
       if(queueShow.Count!=0) queueShow?.Peek()?.action?.Invoke(queueShow.Peek().option);       
    }
    public void DeQueue()
    {
        if (queueShow.Count != 0)
        {
            queueShow?.Dequeue();
            this.ShowAllQueue();
        }     
    }
    public void ShowPopup(PopupOptions option)
    {
        popupDic[option].Appear ();
    }
    public void HidePopup(PopupOptions option)
    {
        popupDic[option].Disappear ();
    }
}
