﻿using Firebase.Database;
using Newtonsoft.Json;
using NSubstitute;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;

public class DataFriendManager : MonoBehaviour
{
    public static Dictionary<string,UserDataServer> friends = new Dictionary<string, UserDataServer>();
    public static Dictionary<string, UserDataServer> friendRequest = new Dictionary<string, UserDataServer>();
    public static Dictionary<string, UserDataServer> friendRequestSent = new Dictionary<string, UserDataServer>();
    public string idAddFriend;
    private bool init = false;

    private void Update()
    {
        if(ServerSystem.databaseRef != null && init == false)
        {
            init = true;
            StartListeningForFriendChanges();
            GetListFriend();
        }
    }
    [ContextMenu("Test Add Friend")]
    public void TestAddFriend()
    {
        AddFriend(idAddFriend);
    }
    public static async void AddFriend(string id)
    {
        try
        {
            var dataSnapshot = await ServerSystem.databaseRef.Child(ServerSystem.USER_DATA_URL + "/" + id).GetValueAsync();
            if (dataSnapshot != null)
            {
                var json = dataSnapshot.GetRawJsonValue();
                UserDataServer userDataServer = JsonConvert.DeserializeObject<UserDataServer>(json);
                UpdateFriend(userDataServer, ServerSystem.user, Friend.State.Sent);
                UpdateFriend(ServerSystem.user, userDataServer, Friend.State.Waiting);
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

    public static async void AcceptFriend(string id)
    {
        try
        {
            var dataSnapshot = await ServerSystem.databaseRef.Child(ServerSystem.USER_DATA_URL + "/" + id).GetValueAsync();
            if (dataSnapshot != null)
            {
                var json = dataSnapshot.GetRawJsonValue();
                UserDataServer userDataServer = JsonConvert.DeserializeObject<UserDataServer>(json);
                UpdateFriend(userDataServer, ServerSystem.user, Friend.State.Confirmed);
                UpdateFriend(ServerSystem.user, userDataServer, Friend.State.Confirmed);

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

    public static async void RemoveFriend(string id)
    {
        try
        {
            var dataSnapshot = await ServerSystem.databaseRef.Child(ServerSystem.USER_DATA_URL + "/" + id).GetValueAsync();
            if (dataSnapshot != null)
            {
                var json = dataSnapshot.GetRawJsonValue();
                UserDataServer userDataServer = JsonConvert.DeserializeObject<UserDataServer>(json);
                RemoveRequest(userDataServer, ServerSystem.user);
                RemoveRequest(ServerSystem.user, userDataServer);

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

    [ContextMenu("Test Start Listening For Friend Changes")]
    public void StartListeningForFriendChanges()
    {
        ServerSystem.databaseRef.Child(ServerSystem.USER_DATA_URL + "/" + ServerSystem.user.id + "/listFriend").ValueChanged += HandleFriendListChanged;
    }
    private async void HandleFriendListChanged(object sender, ValueChangedEventArgs args)
    {
        Debug.Log("La do cai ban be nayyyyyyyyyyyyyyyyyyyyxxxxxxx");
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
                Dictionary<String,Friend> listFriend = JsonConvert.DeserializeObject<Dictionary<String, Friend>>(json);

                friendRequest.Clear();
                friendRequestSent.Clear();
                friends.Clear();
                foreach (Friend friend in listFriend.Values)
                {
                    if (friend != null)
                    {
                        Task<UserDataServer> getFriendTask = GetFriend(friend.id);
                        UserDataServer userDataServer = await getFriendTask;
                        switch (friend.state)
                        {
                            case Friend.State.Waiting:
                                friendRequest[userDataServer.id] = userDataServer;
                                break;
                            case Friend.State.Confirmed:
                                friends[userDataServer.id] = userDataServer;
                                break;
                            case Friend.State.Sent:
                                friendRequestSent[userDataServer.id] = userDataServer;
                                break;
                        }
                    }
                }
            }
            else
            {
                Debug.LogWarning("No data found.");
            }
            FriendManager.Instance?.HandleFriendListChanged();
        }
        catch (Exception ex)
        {
            Debug.LogError($"Error: {ex.Message}");
        }
    }

    private async void GetListFriend()
    {
        try
        {
            var dataSnapshot = await ServerSystem.databaseRef.Child(ServerSystem.USER_DATA_URL + "/" + ServerSystem.user.id + "/listFriend").GetValueAsync();

            if (dataSnapshot != null)
            {
                var json = dataSnapshot.GetRawJsonValue();
                if(string.IsNullOrEmpty(json)) return;
                Dictionary<String, Friend> listFriend = JsonConvert.DeserializeObject<Dictionary<String, Friend>>(json);
                friendRequest.Clear();
                friendRequestSent.Clear();
                friends.Clear();
                // Lặp qua danh sách bạn bè và xử lý khi có trạng thái là Waiting
                foreach (Friend friend in listFriend.Values)
                {
                    if (friend != null)
                    {
                        Task<UserDataServer> getFriendTask = GetFriend(friend.id);
                        UserDataServer userDataServer = await getFriendTask;
                        switch (friend.state)
                        {
                            case Friend.State.Waiting:
                                friendRequest[userDataServer.id] = userDataServer;
                                break;
                            case Friend.State.Confirmed:
                                friends[userDataServer.id] = userDataServer;
                                break;
                            case Friend.State.Sent:
                                friendRequestSent[userDataServer.id] = userDataServer;
                                break;
                        }
                    }
                }
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

    public static void UpdateFriend(UserDataServer friendData, UserDataServer userDataServer, Friend.State state)
    {
        Friend friend = new Friend(state, friendData.id);
        userDataServer.listFriend[friend.id] = friend;
        ServerSystem.SaveToServerAtPath(ServerSystem.USER_DATA_URL + "/" + userDataServer.id, userDataServer);
        Debug.Log("GOi tu UpdateFriend");
        DataUserManager.SaveUserData();
    }

    public static void RemoveRequest(UserDataServer friendData, UserDataServer userDataServer)
    {
        userDataServer.listFriend.Remove(friendData.id);
        ServerSystem.SaveToServerAtPath(ServerSystem.USER_DATA_URL + "/" + userDataServer.id, userDataServer);
        Debug.Log("RemoveRequest");
        DataUserManager.SaveUserData();
    }

    public static async Task<UserDataServer> GetFriend(string id)
    {
        UserDataServer friend = null;

        try
        {
            var dataSnapshot = await ServerSystem.databaseRef.Child(ServerSystem.USER_DATA_URL + "/" + id).GetValueAsync();

            if (dataSnapshot != null && dataSnapshot.Exists)
            {
                var json = dataSnapshot.GetRawJsonValue();
                friend = JsonConvert.DeserializeObject<UserDataServer>(json);
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

        return friend;
    }
}
