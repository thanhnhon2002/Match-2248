using Firebase.Extensions;
using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataUserManager : MonoBehaviour
{
    [ContextMenu("Test Save To Server")]
    public void Test()
    {
        SaveUserData();
    }

    public static void SaveUserData()
    {
        ServerSystem.user.CopyFromLocalData();
        ServerSystem.SaveToServerAtPath(ServerSystem.USER_DATA_URL + "/" + ServerSystem.user.id, ServerSystem.user);
        ServerSaveLoadLocal.SaveToLocal(ServerSystem.user);
    }
}
