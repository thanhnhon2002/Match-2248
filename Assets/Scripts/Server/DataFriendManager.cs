using Firebase.Database;
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
    public async void AddFriend(string id)
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

    [ContextMenu("Test Start Listening For Friend Changes")]
    public void StartListeningForFriendChanges()
    {
        ServerSystem.databaseRef.Child(ServerSystem.USER_DATA_URL + "/" + ServerSystem.user.id + "/listFriend").ValueChanged += HandleFriendListChanged;
    }
    private async void HandleFriendListChanged(object sender, ValueChangedEventArgs args)
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
                Dictionary<String,Friend> listFriend = JsonConvert.DeserializeObject<Dictionary<String, Friend>>(json);

                // Lặp qua danh sách bạn bè và xử lý khi có trạng thái là Waiting
                foreach (Friend friend in listFriend.Values)
                {
                    Task<UserDataServer> getFriendTask = GetFriend(friend.id);
                    UserDataServer userDataServer = await getFriendTask;
                    switch (friend.state)
                    {
                        case Friend.State.Waiting:
                            Debug.LogError(userDataServer.nickName + " Da loi moi ket ban");
                            break;
                        case Friend.State.Confirmed:
                            friends[userDataServer.id] = userDataServer;
                            break;
                        case Friend.State.Sent:
                            break;
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

    private async void GetListFriend()
    {
        try
        {
            var dataSnapshot = await ServerSystem.databaseRef.Child(ServerSystem.USER_DATA_URL + "/" + ServerSystem.user.id + "/listFriend").GetValueAsync();

            if (dataSnapshot != null)
            {
                var json = dataSnapshot.GetRawJsonValue();
                Dictionary<String, Friend> listFriend = JsonConvert.DeserializeObject<Dictionary<String, Friend>>(json);

                // Lặp qua danh sách bạn bè và xử lý khi có trạng thái là Waiting
                foreach (Friend friend in listFriend.Values)
                {
                    Task<UserDataServer> getFriendTask = GetFriend(friend.id);
                    UserDataServer userDataServer = await getFriendTask;
                    switch (friend.state)
                    {
                        case Friend.State.Waiting:
                            Debug.LogError(userDataServer.nickName + " Da loi moi ket ban");
                            break;
                        case Friend.State.Confirmed:
                            friends[userDataServer.id] = userDataServer;
                            break;
                        case Friend.State.Sent:
                            break;
                    }
                }
                friends = friends.OrderByDescending(kv => kv.Value.maxIndex)
                                   .ToDictionary(kv => kv.Key, kv => kv.Value);
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

    public void UpdateFriend(UserDataServer friendData, UserDataServer userDataServer, Friend.State state)
    {
        Friend friend = new Friend(state, friendData.id);
        userDataServer.listFriend[friend.id] = friend;
        ServerSystem.SaveToServerAtPath(ServerSystem.USER_DATA_URL + "/" + userDataServer.id, userDataServer);
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
                Debug.LogWarning("No data found.");
            }
        }
        catch (Exception ex)
        {
            Debug.LogError($"Error: {ex.Message}");
        }

        return friend;
    }
}
