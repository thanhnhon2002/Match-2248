using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ConvertIdGame
{
    public string idSocialNetwork;
    public string idGame;

    public ConvertIdGame(string idSocialNetwork, string idGame)
    {
        this.idSocialNetwork = idSocialNetwork;
        this.idGame = idGame;
    }
}
