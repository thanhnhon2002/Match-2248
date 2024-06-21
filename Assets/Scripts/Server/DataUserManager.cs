using Firebase.Database;
using Firebase.Extensions;
using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class DataUserManager : MonoBehaviour
{
    public static List<UserDataServer> listAllUser = new List<UserDataServer>();
    private bool init;
    private void Update()
    {
        if (ServerSystem.databaseRef != null && init == false)
        {
            init = true;
            StartListeningForUserChanges();
            SaveUserData();
        }
    }

    [ContextMenu("Test Save To Server")]
    public void Test()
    {
        SaveUserData();
    }

    public void StartListeningForUserChanges()
    {
        ServerSystem.databaseRef.Child(ServerSystem.USER_DATA_URL + "/" + ServerSystem.user.id).ValueChanged += HandleUserChanged;
    }

    private void HandleUserChanged(object sender, ValueChangedEventArgs args)
    {
        try
        {
            if (args.DatabaseError != null)
            {
                Debug.LogError(args.DatabaseError.Message);
                return;
            }

            if (args.Snapshot != null && args.Snapshot.Exists)
            {
                // Deserialize dữ liệu snapshot thành đối tượng UserDataServer
                var json = args.Snapshot.GetRawJsonValue();
                ServerSystem.user  = JsonConvert.DeserializeObject<UserDataServer>(json);
                ServerSaveLoadLocal.SaveToLocal(ServerSystem.user);
            }
            else
            {
                Debug.LogWarning("No data found.");
            }
        }
        catch (Exception ex)
        {
            Debug.LogError($"Error: {ex.Message}");
        }
    }

    public static void SaveUserData()
    {
        ServerSystem.user.CopyFromLocalData();
        ServerSystem.SaveToServerAtPath(ServerSystem.USER_DATA_URL + "/" + ServerSystem.user.id, ServerSystem.user);
        ServerSaveLoadLocal.SaveToLocal(ServerSystem.user);
    }

    
}
