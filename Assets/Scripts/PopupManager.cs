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
    RaseGame
}
public class PopupManager : MonoBehaviour
{
    public static PopupManager Instance;
    public Popup popupNewBlock;
    public Popup popupBlockAdded;
    public Popup popupLockElinimated;
    public Popup popupRaseGame;
    public Image blackBackground;
    private void Awake()
    {
        Instance = this;
    }
    public void ShowPopup(PopupOptions option)
    {
        switch (option)
        {
            case PopupOptions.NewBlock:
                popupNewBlock.Appear();  
                break;
            case PopupOptions.BlockAdded:
                popupBlockAdded.Appear();
                break;
            case PopupOptions.LockElinimated:
                popupLockElinimated.Appear();
                break;
            case PopupOptions.RaseGame:
                popupRaseGame.Appear();
                break;
        }
    }
    public void HidePopup(PopupOptions option)
    {
        switch (option)
        {
            case PopupOptions.NewBlock:
                popupNewBlock.Disappear();
                break;
            case PopupOptions.BlockAdded:
                popupBlockAdded.Disappear();
                break;
            case PopupOptions.LockElinimated:
                popupLockElinimated.Disappear();
                break;
            case PopupOptions.RaseGame:
                popupRaseGame.Disappear();
                break;
        }
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.A)) ShowPopup(PopupOptions.NewBlock);
        if (Input.GetKeyDown(KeyCode.S)) HidePopup(PopupOptions.NewBlock);
        if (Input.GetKeyDown(KeyCode.D)) ShowPopup(PopupOptions.BlockAdded);
        if (Input.GetKeyDown(KeyCode.F)) HidePopup(PopupOptions.BlockAdded);
        if (Input.GetKeyDown(KeyCode.G)) ShowPopup(PopupOptions.LockElinimated);
        if (Input.GetKeyDown(KeyCode.H)) HidePopup(PopupOptions.LockElinimated);
        if (Input.GetKeyDown(KeyCode.J)) ShowPopup(PopupOptions.RaseGame);
        if (Input.GetKeyDown(KeyCode.K)) HidePopup(PopupOptions.RaseGame);
    }
}
