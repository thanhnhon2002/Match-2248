using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DarkcupGames;

[Serializable]
public enum PopupOptions
{
    NewBlock,
    BlockAdded,
    LockElinimated,
    RateGame,
    Pause
}
public class PopupManager : MonoBehaviour
{
    public static PopupManager Instance;
    public Popup[] popups;
    private Dictionary<PopupOptions, Popup> popupDic = new Dictionary<PopupOptions, Popup>();
    public Image blackBackground;
    private List<Action<PopupOptions>>listShow=new List<Action<PopupOptions>>();
    private void Awake()
    {
        Instance = this;
        popups = GetComponentsInChildren<Popup>(true);
        foreach (var item in popups)
        {
            popupDic.Add(item.option, item);
        }
    }
    public void AddActionShowPopup(Action<PopupOptions> action,PopupOptions option)
    {
        listShow.Add(action);
        this.Show();
    }
    private void Show()
    {
        foreach(var action in listShow)
        {
            //action?.Invoke();
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

#if UNITY_EDITOR
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.A)) ShowPopup(PopupOptions.NewBlock);
        if (Input.GetKeyDown(KeyCode.S)) HidePopup(PopupOptions.NewBlock);

        if (Input.GetKeyDown(KeyCode.D)) ShowPopup(PopupOptions.RateGame);
        if (Input.GetKeyDown(KeyCode.F)) HidePopup(PopupOptions.RateGame);

        if (Input.GetKeyDown(KeyCode.G)) ShowPopup(PopupOptions.LockElinimated);
        if (Input.GetKeyDown(KeyCode.H)) HidePopup(PopupOptions.LockElinimated);

        if (Input.GetKeyDown(KeyCode.J)) ShowPopup(PopupOptions.Pause);
        if (Input.GetKeyDown(KeyCode.K)) HidePopup(PopupOptions.Pause);
    }
#endif
}
