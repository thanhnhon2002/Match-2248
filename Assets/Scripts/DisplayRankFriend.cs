using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class DisplayRankFriend : MonoBehaviour
{
    [SerializeField] private Transform content;
    [SerializeField] private RankUserInfo rankUserInfo;
    [SerializeField] private CanvasGroup canvasGroup;

    private void OnEnable()
    {
        SetUpContent();
    }

    private void OnDisable()
    {
        foreach(RankUserInfo userInfo in GetComponentsInChildren<RankUserInfo>())
        {
            userInfo.gameObject.SetActive(false);
        }
    }

    public async void SetUpContent()
    {
        Dictionary<string, UserDataServer> rankFriend = new Dictionary<string, UserDataServer>(DataFriendManager.friends);
        rankFriend[ServerSystem.user.id] = ServerSystem.user;
        rankFriend = rankFriend.OrderByDescending(kv => kv.Value.maxIndex).ToDictionary(kv => kv.Key, kv => kv.Value);
        foreach (UserDataServer user in rankFriend.Values)
        {
            var info = PoolSystem.Instance.GetObjectFromPool(rankUserInfo, content);
            await info.DisplayInfo(user);
        }
    }
}
