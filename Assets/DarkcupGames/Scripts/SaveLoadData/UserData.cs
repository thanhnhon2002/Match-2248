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
    public long lastRewardTick;
    public List<bool> hasClaim = new List<bool>();

    public DailyRewardInfo()
    {
        lastRewardTick = 0;
        hasClaim = new List<bool>();
        for (int i = 0; i < 5; i++)
        {
            hasClaim.Add(false);
        }
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
    public Dictionary<string, BigInteger> cellDic = new Dictionary<string, BigInteger>();

    public GameData()
    {
        cellDic = new Dictionary<string, BigInteger>();
        currentHighestCellValue = 0;
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
    public float gold;
    public float diamond;
    public bool replay;
    public bool firstPlayGame;
    public BigInteger highestScore;
    public BigInteger highestCellValue;
    public GameData gameData;
    public UserProperty property;
    public DailyRewardInfo dailyRewardInfo;
    public Dictionary<SettingKey, bool> dicSetting = new Dictionary<SettingKey, bool>();
    public List<string> boughtItems;

    public UserData()
    {
        diamond = 500;
        property = new UserProperty();
        gameData = new GameData();
        dailyRewardInfo = new DailyRewardInfo();
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