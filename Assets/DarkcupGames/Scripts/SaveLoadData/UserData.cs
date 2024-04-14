using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum SettingKey
{
    Sound, Music, Vibration
}

[System.Serializable]
public class UserData
{
    public float gold;
    public float diamond;
    public int level;
    public int maxLevel;
    public Dictionary<SettingKey, bool> dicSetting = new Dictionary<SettingKey, bool>();
    public List<string> boughtItems;

    public UserData()
    {
        boughtItems = new List<string>();
    }
    public void CheckValid()
    {
        if (boughtItems == null) boughtItems = new List<string>();
        if (dicSetting == null) dicSetting = new Dictionary<SettingKey, bool>();
        if (dicSetting.ContainsKey(SettingKey.Sound) == false) dicSetting.Add(SettingKey.Sound, true);
        if (dicSetting.ContainsKey(SettingKey.Music) == false) dicSetting.Add(SettingKey.Music, true);
        if (dicSetting.ContainsKey(SettingKey.Vibration) == false) dicSetting.Add(SettingKey.Vibration, true);
    }
}