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
    public static DataUserManager Instance;
    public static List<UserDataServer> listAllUser = new List<UserDataServer>();
    private bool init;

    private void Awake()
    {
        Instance = this;
    }
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

    public void RemoveListeningForUserChanges()
    {
        ServerSystem.databaseRef.Child(ServerSystem.USER_DATA_URL + "/" + ServerSystem.user.id).ValueChanged -= HandleUserChanged;
    }

    private void HandleUserChanged(object sender, ValueChangedEventArgs args)
    {
        Debug.Log("Bi thay doi cai gi do: " + ServerSystem.user.id);
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
                UserDataServer userDataServer  = JsonConvert.DeserializeObject<UserDataServer>(json);
                if (userDataServer.id != ServerSystem.user.id) 
                {
                    Debug.Log("Tk bi thay doi" + "sever: " + userDataServer.id + "local: " + ServerSystem.user.id);
                    return;
                }
                Debug.Log("Tai sao lai chay cai nay" + "sever: " + userDataServer.id + "local: " + ServerSystem.user.id);
                ServerSaveLoadLocal.SaveToLocal(userDataServer);
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
        Debug.Log("GOi tu DataUserManager: " + ServerSystem.user.id);
        ServerSystem.user.CopyFromLocalData();
        ServerSystem.SaveToServerAtPath(ServerSystem.USER_DATA_URL + "/" + ServerSystem.user.id, ServerSystem.user);
        ServerSaveLoadLocal.SaveToLocal(ServerSystem.user);
        Debug.Log("GOi sau coppy DataUserManager: " + ServerSystem.user.id);
    }

    
}
