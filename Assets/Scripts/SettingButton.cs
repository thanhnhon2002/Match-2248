using DarkcupGames;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SettingButton : MonoBehaviour
{
    [SerializeField] private SettingKey key;
    [SerializeField] private Image icon;
    [SerializeField] private Sprite[] icons;

    private void OnEnable ()
    {
        var userData = GameSystem.userdata;
        var index = userData.dicSetting[key] ? 1 : 0;
        icon.sprite = icons[index];
    }

    public void OnClick()
    {
        var userData = GameSystem.userdata;
        userData.dicSetting[key] = !userData.dicSetting[key];
        var index = userData.dicSetting[key] ? 1 : 0;
        icon.sprite = icons[index];
        GameSystem.SaveUserDataToLocal ();
    }
}
