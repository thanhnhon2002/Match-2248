using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Friend
{
    public enum State
    {
        Confirmed, Waiting, Sent
    }
    public State state;
    public string id;
    public Friend(State state, string id)
    {
        this.id = id;
        this.state = state;
    }
}