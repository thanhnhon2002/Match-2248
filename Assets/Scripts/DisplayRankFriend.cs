using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class DisplayRankFriend : MonoBehaviour
{
    [SerializeField] private Transform content;
    [SerializeField] private RankUserInfo rankUserInfo;
    [SerializeField] private RankUserInfo myInfo;
    [SerializeField] private CanvasGroup canvasGroup;

    private void OnEnable()
    {
        SetUpContent();
    }

    private void OnDisable()
    {
        foreach(RankUserInfo userInfo in content.GetComponentsInChildren<RankUserInfo>())
        {
            userInfo.gameObject.SetActive(false);
        }
    }

    public void OnClickRankFriend()
    {
        SetUpContent();
    }

    public async void SetUpContent()
    {
        ResetContent();
        Dictionary<string, UserDataServer> rankFriend = new Dictionary<string, UserDataServer>(DataFriendManager.friends);
        rankFriend[ServerSystem.user.id] = ServerSystem.user;
        rankFriend = rankFriend.OrderByDescending(kv => kv.Value.maxIndex).ToDictionary(kv => kv.Key, kv => kv.Value);
        foreach (UserDataServer user in rankFriend.Values)
        {
            if (user.id == ServerSystem.user.id)
            {
                var info = PoolSystem.Instance.GetObjectFromPool(myInfo, content);
            }
            else
            {
                var info = PoolSystem.Instance.GetObjectFromPool(rankUserInfo, content);
                await info.DisplayInfo(user);
            }
        }
    }

    private void ResetContent()
    {
        foreach(var info in content.GetComponentsInChildren<RankUserInfo>())
        {
            info.gameObject.SetActive(false);
        }
    }
}
