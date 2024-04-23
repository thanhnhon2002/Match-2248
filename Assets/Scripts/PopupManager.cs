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
    RateGame
}
public class PopupManager : MonoBehaviour
{
    public static PopupManager Instance;
    public Popup[] popups;
    private Dictionary<PopupOptions, Popup> popupDic = new Dictionary<PopupOptions, Popup>();
    public Image blackBackground;
    public event Action<PopupOptions> popupShow;
    private void Awake()
    {
        Instance = this;
        popups = GetComponentsInChildren<Popup>();
        foreach (var item in popups)
        {
            popupDic.Add (item.option, item);
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

        if (Input.GetKeyDown(KeyCode.D)) ShowPopup(PopupOptions.BlockAdded);
        if (Input.GetKeyDown(KeyCode.F)) HidePopup(PopupOptions.BlockAdded);

        if (Input.GetKeyDown(KeyCode.G)) ShowPopup(PopupOptions.LockElinimated);
        if (Input.GetKeyDown(KeyCode.H)) HidePopup(PopupOptions.LockElinimated);

        if (Input.GetKeyDown(KeyCode.J)) ShowPopup(PopupOptions.RateGame);
        if (Input.GetKeyDown(KeyCode.K)) HidePopup(PopupOptions.RateGame);
    }
#endif
}
