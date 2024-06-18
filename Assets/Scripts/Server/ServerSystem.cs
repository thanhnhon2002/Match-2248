using DarkcupGames;
using DG.Tweening.Plugins.Core.PathCore;
using Firebase.Database;
using Firebase.Extensions;
using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using UnityEngine;

public class ServerSystem : MonoBehaviour
{
    public const string BASE_URL = "game";
    public const string USER_DATA_URL = BASE_URL + "/user";
    public const string RANK_DATA_URL = BASE_URL + "/rank";

    public static ServerSystem Instance;
    public static DatabaseReference databaseRef;
    private static UserDataServer user = new UserDataServer();
    private static bool init = false;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Update()
    {
        if (FirebaseManager.Instance.ready == false && init == false)
        {
            init = true;
            user = ServerSaveLoadLocal.LoadUserDataFromLocal();
            databaseRef = FirebaseDatabase.DefaultInstance.RootReference;
        }
    }

    [ContextMenu("Test Save To Server")]
    public void Test()
    {
        SaveUserData();
    }

    public static void SaveUserData()
    {
        user.CopyFromLocalData();
        UpdateListFriend();
        SaveToServerAtPath(USER_DATA_URL + "/" + user.id, user);
        ServerSaveLoadLocal.SaveToLocal(user);
    }

    private static void SaveToServerAtPath(string path, object value)
    {
        if (init == false || databaseRef == null) return;

        string json = JsonConvert.SerializeObject(value);

        databaseRef.Child(path).SetRawJsonValueAsync(json).ContinueWithOnMainThread(task =>
        {
            if (task.Status != System.Threading.Tasks.TaskStatus.RanToCompletion)
            {
                Debug.LogError($"save to server failed with status {task.Status}, exception = {task.Exception}");
            } else
            {
                Debug.Log("save to server success");
            }
        });
    }

    public static void UpdateListFriend()
    {
        if (user.listFriend.Count == 0)
        {
            AddSelfAsFriend();
        }
    }

    public static void AddSelfAsFriend()
    {
        Friend friend = new Friend(Friend.State.Confirmed, user.id, user.nickName, user.avatarPath, user.indexPlayer, user.maxIndex, user.avatarIndex);
        user.listFriend[friend.id] = friend;
    }

    public static void UpdateRank()
    {
        SaveToServerAtPath(RANK_DATA_URL + "/" + user.maxIndex + "/" + user.id, user);
    }
}