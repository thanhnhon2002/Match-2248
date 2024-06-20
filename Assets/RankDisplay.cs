using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Unity.Collections;
using UnityEngine;
using UnityEngine.UIElements;

public class RankDisplay : MonoBehaviour
{
    private RankUserInfo[] infos;
    [ReadOnly] public Sprite[] avatar;
    [SerializeField] private GameObject loadingIcon;
    [SerializeField] private CanvasGroup rankGroup;
    private List<UserDataServer> userDataServers = new List<UserDataServer>();

    private void Awake()
    {
        infos = GetComponentsInChildren<RankUserInfo>();
    }

    private async void Start()
    {
        await GetUsers();
        await DisplayRank();
    }

    private async Task GetUsers()
    {
        userDataServers.Clear();
        if (ServerSystem.rank.topTenRank.Count == 0) await DataRankManager.GetRankGlobal();
        userDataServers.AddRange(ServerSystem.rank.topTenRank);
        userDataServers.Sort((a,b) => b.indexPlayer.CompareTo(a.indexPlayer));
    }

    private async Task DisplayRank()
    {
        for (int i = 0; i < infos.Length; i++)
        {
            infos[i].gameObject.SetActive(false);
        }
        loadingIcon.SetActive(true);
        rankGroup.alpha = 0f;
        rankGroup.interactable = false;
        for (int i = 0; i < userDataServers.Count; i++)
        {
            await infos[i].DisplayInfo(userDataServers[i]);
            infos[i].gameObject.SetActive(true);
        }
        loadingIcon.SetActive(false);
        rankGroup.alpha = 1f;
        rankGroup.interactable = true;
    }
}
