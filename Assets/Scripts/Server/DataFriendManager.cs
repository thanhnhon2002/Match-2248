using Firebase.Database;
using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataFriendManager : MonoBehaviour
{
    public string idAddFriend;

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
        ServerSystem.databaseRef.Child(ServerSystem.USER_DATA_URL + "/" + ServerSystem.user.id).ValueChanged += HandleFriendListChanged;
    }
    private void HandleFriendListChanged(object sender, ValueChangedEventArgs args)
    {
        Debug.Log("aaa");
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
                UserDataServer userDataServer = JsonConvert.DeserializeObject<UserDataServer>(json);

                // Lặp qua danh sách bạn bè và xử lý khi có trạng thái là Waiting
                foreach (Friend friend in userDataServer.listFriend.Values)
                {
                    if (friend.state.Equals(Friend.State.Waiting))
                    {

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

    public void UpdateFriend(UserDataServer friendData, UserDataServer userDataServer, Friend.State state)
    {
        Friend friend = new Friend(state, friendData.id);
        userDataServer.listFriend[friend.id] = friend;
        ServerSystem.SaveToServerAtPath(ServerSystem.USER_DATA_URL + "/" + userDataServer.id, userDataServer);
    }
}
