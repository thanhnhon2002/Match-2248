using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
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
        foreach(UserDataServer user in DataFriendManager.friends.Values)
        {
            var info = PoolSystem.Instance.GetObjectFromPool(rankUserInfo, content);
            await info.DisplayInfo(user);
        }
    }
}
