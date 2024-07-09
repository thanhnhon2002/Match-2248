using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ConvertIdManager : MonoBehaviour
{
    [ContextMenu("TestConvertID")]
    public void TestConvertId()
    {
        UpdateIdConvert("112064091026792404297", "eo58uu6i");
    }

    [ContextMenu("UpdateIdConvert")]
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
                if (idGame.Equals(convertIdGame))
                {
                    UserDataServer.UpdateLocalData(dataServer);
                }
                else
                {
                    PopupNotification.Instance.ShowPopupYesNo("Are you sure you want to change account?", () =>
                    {
                        UserDataServer.UpdateLocalData(dataServer);
                        Destroy(ServerSystem.Instance.gameObject);
                        SceneManager.LoadScene("Loading");
                    });
                }
            }
            else
            {
                PopupNotification.Instance.ShowPopupYesNo("Your highest score is " + currentUser.hightScore +  " and the highest score of the account linked to your Google account is " + dataServer.hightScore + ". Do you want to replace it?", () =>
                {
                    ConvertIdGame convertId = new ConvertIdGame(idSocialNetwork, idGame);
                    ServerSystem.SaveToServerAtPath(ServerSystem.CONVERT_ID_URL + "/" + idSocialNetwork, convertId);
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
