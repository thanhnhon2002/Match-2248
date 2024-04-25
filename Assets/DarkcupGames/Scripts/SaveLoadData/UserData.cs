using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Numerics;

public enum SettingKey
{
    Sound, Music, Vibration
}

[System.Serializable]
public class UserData
{
    public string nickName;
    public int avatarIndex;
    public float gold;
    public float diamond;
    public BigInteger highestScore;
    public BigInteger currentScore;
    public BigInteger highestCellValue;
    public int indexPlayer;
    public int minIndex;
    public int maxIndex;
    public int maxIndexRandom;
    public Dictionary<string, BigInteger> cellDic = new Dictionary<string, BigInteger>();
    public Dictionary<SettingKey, bool> dicSetting = new Dictionary<SettingKey, bool>();
    public List<string> boughtItems;

    public UserData()
    {
        nickName = $"Guest{UnityEngine.Random.Range (1, int.MaxValue)}";
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