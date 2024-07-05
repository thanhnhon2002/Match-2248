using System;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;



[System.Serializable]
public class UserDataServer
{
    public enum TypeLogin
    {
        Guest, Google, Facebook, Apple
    }
    public string nickName;
    public string id;
    public string idGoogle;
    public string idFacebook;
    public string idApple;
    public string avatarPath;
    public int indexPlayer;
    public int maxIndex;
    public int avatarIndex;
    public BigInteger hightScore;
    public BigInteger highestCellValue;
    public TypeLogin typeLogin;
    public Dictionary<string, Friend> listFriend = new Dictionary<string, Friend>();
    public GameData gameData;
    public List<string> boughtItems;
    public void CopyFromLocalData()
    {
        if (GameSystem.userdata == null)
        {
            Debug.LogError("user data currently is null");
            return;
        }
        if (GameSystem.userdata.gameData == null)
        {
            Debug.LogError("user game data null");
            return;
        }

        this.maxIndex = GameSystem.userdata.gameData.maxIndex;
        this.indexPlayer = GameSystem.userdata.gameData.indexPlayer;
        this.nickName = GameSystem.userdata.nickName;
        this.avatarIndex = GameSystem.userdata.avatarIndex;
        this.hightScore = GameSystem.userdata.highestScore;
        this.highestCellValue = GameSystem.userdata.highestCellValue;
        this.gameData = GameSystem.userdata.gameData;
        //if (this.gameData.cellDic == null)
        //{
        //    this.gameData.cellDic = new Dictionary<string, int>();
        //}
        //var newDic = new Dictionary<string, int>();
        //foreach(var item in this.gameData.cellDic)
        //{
        //    newDic.Add(item.Key.Replace(".", "_"), item.Value);
        //}
        //this.gameData.cellDic = newDic;
        this.boughtItems = GameSystem.userdata.boughtItems;
    }

    public static void UpdateLocalData(UserDataServer dataServer)
    {
        if (GameSystem.userdata == null)
        {
            Debug.LogError("user data currently is null");
            return;
        }
        if (GameSystem.userdata.gameData == null)
        {
            Debug.LogError("user game data null");
            return;
        }

        GameSystem.userdata.gameData.maxIndex = dataServer.maxIndex;
        GameSystem.userdata.gameData.indexPlayer = dataServer.indexPlayer;
        GameSystem.userdata.nickName = dataServer.nickName;
        GameSystem.userdata.avatarIndex = dataServer.avatarIndex;
        GameSystem.userdata.highestScore = dataServer.hightScore;
        GameSystem.userdata.highestCellValue = dataServer.highestCellValue;
        GameSystem.userdata.gameData = dataServer.gameData;
        GameSystem.userdata.boughtItems = dataServer.boughtItems;
        GameSystem.SaveUserDataToLocal();
    }

    public string GetID()
    {
        return typeLogin switch
        {
            TypeLogin.Guest => id,
            TypeLogin.Google => idGoogle,
            TypeLogin.Facebook => idFacebook,
            TypeLogin.Apple => idApple,
            _=> ""
        };
    }
}
