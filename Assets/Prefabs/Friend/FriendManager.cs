using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

public class FriendManager : MonoBehaviour
{
    public static FriendManager Instance;

    [SerializeField] Transform content;
    [SerializeField] FriendInfo friendInfo;
    [SerializeField] FriendInfo friendRequestInfo;
    [SerializeField] Image[] maskButton;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }

    private async void OnEnable()
    {
        ResetContent();
        await ShowAllFriendAsync();
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
        UserDataServer userData = null;
        if (id.Equals(""))
        {
            await ShowAllFriendAsync();
        }
        else if (DataFriendManager.friends.ContainsKey(id))
        {
            userData = DataFriendManager.friends[id];
        }
        else
        {
            userData = await DataFriendManager.GetFriend(id);
        }
        ResetContent();
        if (userData != null)
        {
            var info = PoolSystem.Instance.GetObjectFromPool(friendInfo, content);
            await info.DisplayInfo(userData);
        }
        
    }

    public async Task ShowAllFriendRequest()
    {
        foreach (UserDataServer user in DataFriendManager.friendRequest.Values)
        {
            var info = PoolSystem.Instance.GetObjectFromPool(friendRequestInfo, content);
            await info.DisplayInfoRequest(user);
        }
    }

    public void ResetContent()
    {
        foreach(FriendInfo friendInfo in content.GetComponentsInChildren<FriendInfo>())
        {
            friendInfo.gameObject.SetActive(false);
        }
    }

    public async void CickListFriend()
    {
        maskButton[0].gameObject.SetActive(true);
        maskButton[1].gameObject.SetActive(false);
        ResetContent();
        await ShowAllFriendAsync();
    }

    public async void CickListFriendRequest()
    {
        maskButton[1].gameObject.SetActive(true);
        maskButton[0].gameObject.SetActive(false);
        ResetContent();
        await ShowAllFriendRequest();
    }
}
