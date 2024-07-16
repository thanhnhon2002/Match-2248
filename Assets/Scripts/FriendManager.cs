using DG.Tweening;
using Firebase.Database;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class FriendManager : MonoBehaviour
{
    public enum NameContent
    {
        AllFriend, Search, FriendRequest
    }

    public static FriendManager Instance;

    [SerializeField] Transform content;
    [SerializeField] UserInfo userInfo;
    [SerializeField] FriendRequestInfo friendRequestInfo;
    [SerializeField] SentFriendRequestInfo sentFriendRequestInfo;
    [SerializeField] FriendInfo friendInfo;
    [SerializeField] TextMeshProUGUI[] texts;
    [SerializeField] TMP_InputField inputField;
    private NameContent currentContent;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }

    private async void OnEnable()
    {
        ClickListFriend();
        ResetContent();
        await ShowAllFriendAsync();
    }

    public async Task ShowAllFriendAsync()
    {
        currentContent = NameContent.AllFriend;
        foreach (UserDataServer user in DataFriendManager.friends.Values)
        {
            var info = PoolSystem.Instance.GetObjectFromPool(friendInfo, content);
            await info.DisplayInfo(user);
        }
    }

    public async Task SearchUser(string id)
    {
        if (currentContent.Equals(NameContent.FriendRequest)) return;
        ResetContent();
        currentContent = NameContent.Search;
        UserDataServer userData = null;
        if (id.Equals(""))
        {
            await ShowAllFriendAsync();
        }
        else if (DataFriendManager.friends.ContainsKey(id))
        {
            userData = DataFriendManager.friends[id];
            var info = PoolSystem.Instance.GetObjectFromPool(friendInfo, content);
            await info.DisplayInfo(userData);
        }
        else if (DataFriendManager.friendRequestSent.ContainsKey(id))
        {
            userData = DataFriendManager.friendRequestSent[id];
            var info = PoolSystem.Instance.GetObjectFromPool(sentFriendRequestInfo, content);
            await info.DisplayInfo(userData);
        }
        else
        {
            userData = await DataFriendManager.GetFriend(id);
            if (userData != null && userData.id!= ServerSystem.user.id)
            {
                var info = PoolSystem.Instance.GetObjectFromPool(userInfo, content);
                await info.DisplayInfo(userData);
            }
        }
        
    }

    public async Task ShowAllFriendRequest()
    {
        currentContent = NameContent.FriendRequest;
        foreach (UserDataServer user in DataFriendManager.friendRequest.Values)
        {
            var info = PoolSystem.Instance.GetObjectFromPool(friendRequestInfo, content);
            await info.DisplayInfo(user);
        }
    }

    public void ResetContent()
    {
        if (content == null) return;
        foreach(UserInfo friendInfo in content.GetComponentsInChildren<UserInfo>())
        {
            friendInfo.gameObject.SetActive(false);
        }
        foreach (FriendInfo friendInfo in content.GetComponentsInChildren<FriendInfo>())
        {
            friendInfo.gameObject.SetActive(false);
        }
        foreach (SentFriendRequestInfo friendInfo in content.GetComponentsInChildren<SentFriendRequestInfo>())
        {
            friendInfo.gameObject.SetActive(false);
        }
        foreach (FriendRequestInfo friendInfo in content.GetComponentsInChildren<FriendRequestInfo>())
        {
            friendInfo.gameObject.SetActive(false);
        }
    }

    public async void ClickListFriend()
    {
        inputField.text = "";
        texts[0].DOFade(1, 0);
        texts[1].DOFade(0.1f, 0);
        ResetContent();
        await ShowAllFriendAsync();
    }

    public async void ClickListFriendRequest()
    {
        inputField.text = "";
        texts[1].DOFade(1, 0);
        texts[0].DOFade(0.1f, 0);
        ResetContent();
        await ShowAllFriendRequest();
    }

    public async void HandleFriendListChanged()
    {
        switch (currentContent)
        {
            case NameContent.AllFriend:
                await ShowAllFriendAsync();
                break;
            case NameContent.Search:
                await SearchUser(inputField.text);
                break;
            case NameContent.FriendRequest:
                await ShowAllFriendRequest();
                break;
        }

        ResetContent();
    }
}
