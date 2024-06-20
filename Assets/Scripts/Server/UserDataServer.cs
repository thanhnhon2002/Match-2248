using System;
using System.Collections.Generic;
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
    public TypeLogin typeLogin;
    public Dictionary<string, Friend> listFriend = new Dictionary<string, Friend>();
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
