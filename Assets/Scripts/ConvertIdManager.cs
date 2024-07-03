using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConvertIdManager : MonoBehaviour
{
    public static void UpdateIdConvert(string idSocialNetwork, string idGame)
    {
        ConvertIdGame convertId = new ConvertIdGame();
        convertId.idSocialNetwork = idSocialNetwork;
        convertId.idGame = idGame;
        ServerSystem.SaveToServerAtPath(ServerSystem.CONVERT_ID_URL + "/" + idSocialNetwork, convertId);
    }
}
