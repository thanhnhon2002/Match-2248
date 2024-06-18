using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Friend
{
    public enum State
    {
        Confirmed, Waiting
    }
    public State state;
    public string id;
    public string nickName;
    public string avatarPath;
    public int indexPlayer;
    public int maxIndex;
    public int avatarIndex;


    public Friend(State state, string id, string nickName, string avatarPath, int indexPlayer, int maxIndex, int avatarIndex)
    {
        this.id = id;
        this.state = state;
        this.nickName = nickName;
        this.avatarPath = avatarPath;
        this.indexPlayer = indexPlayer;
        this.maxIndex = maxIndex;
        this.avatarIndex = avatarIndex;
    }
}