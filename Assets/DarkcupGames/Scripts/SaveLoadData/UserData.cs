using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Numerics;
using JetBrains.Annotations;

public enum SettingKey
{
    Sound, Music, Vibration
}

[System.Serializable]
public class UserProperty
{
    public int level_max;
    public int last_level;
    public string last_placement;
    public int total_interstitial_ads;
    public int total_rewarded_ads;
}

[System.Serializable]
public class DailyRewardInfo
{
    public DateTime lastFreeClaimTime;
    public double freeTimeRemain;
    public bool[] hasClaim = new bool[] { false, false, false, false, false};
    public DailyRewardInfo()
    {
        freeTimeRemain = 0;
    }
}

[System.Serializable]
public class GameData
{
    public BigInteger currentScore;
    public BigInteger currentHighestCellValue;
    public int indexPlayer;
    public int minIndex;
    public int maxIndex;
    public int maxIndexRandom;
    public Dictionary<string, int> cellDic = new Dictionary<string, int>();

    public GameData()
    {
        cellDic = new Dictionary<string, int>();
        currentHighestCellValue = 128;
        currentScore = 0;
        indexPlayer = 0;
        minIndex = 0;
        maxIndex = 0;
        maxIndexRandom = 0;
    }
}

[System.Serializable]
public class UserData
{
    public string nickName;
    public int avatarIndex;
    public int level;
    public float gold;
    public float diamond;
    public bool replay;
    public bool firstPlayGame;
    public long lastSpecialOffer;
    public BigInteger highestScore;
    public BigInteger highestCellValue;
    public GameData gameData;
    public UserProperty property;
    public DailyRewardInfo dailyRewardInfo;
    public Dictionary<SettingKey, bool> dicSetting = new Dictionary<SettingKey, bool>();
    public List<string> boughtItems;
    public int lastHighestCellValue;
    public UserData()
    {
#if UNITY_EDITOR
        diamond = 10000;
#endif
        highestCellValue = 128;
        property = new UserProperty();
        gameData = new GameData();
        var r = new System.Random();
        nickName = $"Guest{r.Next(0, int.MaxValue)}";
        boughtItems = new List<string>();
        firstPlayGame = true;
        level = 0;
    }
    public void CheckValid()
    {
        if (dailyRewardInfo == null) dailyRewardInfo = new DailyRewardInfo();
        if (boughtItems == null) boughtItems = new List<string>();
        if (dicSetting == null) dicSetting = new Dictionary<SettingKey, bool>();
        if (dicSetting.ContainsKey(SettingKey.Sound) == false) dicSetting.Add(SettingKey.Sound, true);
        if (dicSetting.ContainsKey(SettingKey.Music) == false) dicSetting.Add(SettingKey.Music, true);
        if (dicSetting.ContainsKey(SettingKey.Vibration) == false) dicSetting.Add(SettingKey.Vibration, true);
    }
}