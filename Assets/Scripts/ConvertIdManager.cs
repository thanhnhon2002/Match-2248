using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class ConvertIdManager : MonoBehaviour
{
    [ContextMenu("TestConvertID")]
    public void TestConvertId()
    {
        UpdateIdConvert("A", "2x9xa4bt");
    }
    public static async void UpdateIdConvert(string idSocialNetwork, string idGame)
    {
        Debug.Log("a");
        string convertIdGame = await GetIdGameByIdSocialNetwork(idSocialNetwork);
        if (convertIdGame == null)
        {
            Debug.Log(idSocialNetwork + "g" +  idGame);
            ConvertIdGame convertId = new ConvertIdGame(idSocialNetwork, idGame);
            ServerSystem.SaveToServerAtPath(ServerSystem.CONVERT_ID_URL + "/" + idSocialNetwork, convertId);
        }
        else
        {
            UserDataServer dataServer = await GetUserByIdGame(convertIdGame);

            UserDataServer currentUser = ServerSystem.user;
            if (currentUser.typeLogin != UserDataServer.TypeLogin.Guest) 
            {
                PopupNotification.Instance.ShowPopupYesNo("Are you sure you want to change account?", () =>
                {
                    UserDataServer.UpdateLocalData(dataServer);
                });
            }
            else
            {
                PopupNotification.Instance.ShowPopupYesNo("", () =>
                {
                    UserDataServer.UpdateLocalData(dataServer);
                });
            }        
        }
    }

    public static async Task<String> GetIdGameByIdSocialNetwork(string idSocialNetwork)
    {
        string idGame = null;

        try
        {
            var dataSnapshot = await ServerSystem.databaseRef.Child(ServerSystem.CONVERT_ID_URL + "/" + idSocialNetwork).GetValueAsync();

            if (dataSnapshot != null && dataSnapshot.Exists)
            {
                var json = dataSnapshot.GetRawJsonValue();
                var idConvert = JsonConvert.DeserializeObject<ConvertIdGame>(json);
                idGame = idConvert.idGame;
            }
            else
            {
                // Debug.LogWarning("No data found.");
            }
        }
        catch (Exception ex)
        {
            Debug.LogError($"Error: {ex.Message}");
        }

        return idGame;
    }

    public static async Task<UserDataServer> GetUserByIdGame(string idGame)
    {
        UserDataServer userDataServer = null;

        try
        {
            var dataSnapshot = await ServerSystem.databaseRef.Child(ServerSystem.USER_DATA_URL + "/" + idGame).GetValueAsync();

            if (dataSnapshot != null && dataSnapshot.Exists)
            {
                var json = dataSnapshot.GetRawJsonValue();
                userDataServer = JsonConvert.DeserializeObject<UserDataServer>(json);
            }
            else
            {
                // Debug.LogWarning("No data found.");
            }
        }
        catch (Exception ex)
        {
            Debug.LogError($"Error: {ex.Message}");
        }

        return userDataServer;
    }

    public static async void RemoveConvertId(string idSocialNetwork)
    {
        try
        {
            await ServerSystem.databaseRef.Child(ServerSystem.CONVERT_ID_URL + "/" + idSocialNetwork).RemoveValueAsync();
        }
        catch (Exception ex)
        {
            Debug.LogError($"Error: {ex.Message}");
        }
    }
}
