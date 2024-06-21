using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class FriendManager : MonoBehaviour
{
    public static FriendManager Instance;

    [SerializeField] Transform content;
    [SerializeField] FriendInfo friendInfo;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }

    public async Task ShowAllFriendAsync()
    {
        foreach (UserDataServer user in DataFriendManager.friends.Values)
        {
            var info = PoolSystem.Instance.GetObjectFromPool(friendInfo, content);
            await info.DisplayInfo(user);
        }
    }

    public async Task SearchUser(string id)
    {
        UserDataServer userData;
        if (DataFriendManager.friends.ContainsKey(id))
        {
            userData = DataFriendManager.friends[id];
        }
        else
        {
            userData = await DataFriendManager.GetFriend(id);
        }
        ResetContent();
        var info = PoolSystem.Instance.GetObjectFromPool(friendInfo, content);
        await info.DisplayInfo(userData);
    }

    public void ResetContent()
    {
        foreach(FriendInfo friendInfo in content.GetComponentsInChildren<FriendInfo>())
        {
            friendInfo.gameObject.SetActive(false);
        }
    }
}
