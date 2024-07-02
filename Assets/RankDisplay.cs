using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using TMPro;
using Unity.Collections;
using UnityEngine;
using DG.Tweening;

public class RankDisplay : MonoBehaviour
{
    [SerializeField] private RankUserInfo[] infos;
    [ReadOnly] public Sprite[] avatar;
    [SerializeField] private PersonalRank personal;
    [SerializeField] private GameObject loadingIcon;
    [SerializeField] private CanvasGroup rankGroup;
    [SerializeField] private TextMeshProUGUI message;
    [SerializeField] private TextMeshProUGUI[] texts;
    private List<UserDataServer> userDataServers = new List<UserDataServer>();
    public List<UserDataServer> UserDataServers => userDataServers;

    private async void Start()
    {
        message.text = "Loading\nPlease wait";
        await DataRankManager.GetRankGlobal(DisplayRank, DisplayFailMessage);
        personal.DisplayPersonalRank();
    }

    private async void DisplayRank(List<UserDataServer> users)
    {
        userDataServers.Clear();
        userDataServers.AddRange(users);
        userDataServers.Sort((a, b) => b.indexPlayer.CompareTo(a.indexPlayer));

        for (int i = 0; i < infos.Length; i++)
        {
            infos[i].gameObject.SetActive(false);
        }

        loadingIcon.SetActive(true);
        message.gameObject.SetActive(true);
        rankGroup.alpha = 0f;
        rankGroup.interactable = false;
        if (userDataServers.Count == 0) return;
        var count = infos.Length;
        if(count > userDataServers.Count) count = userDataServers.Count;

        for (int i = 0; i < count; i++)
        {
            await infos[i].DisplayInfo(userDataServers[i]);
            infos[i].gameObject.SetActive(true);
        }

        loadingIcon.SetActive(false);
        message.gameObject.SetActive(false);
        rankGroup.alpha = 1f;
        rankGroup.interactable = true;
    }

    private void DisplayFailMessage()
    {
        message.gameObject.SetActive(true);
        for (int i = 0; i < infos.Length; i++)
        {
            infos[i].gameObject.SetActive(false);
        }
        message.text = "Please try again later";
    }

    public void ClickRankGlobal()
    {
        texts[0].DOFade(1, 0);
        texts[1].DOFade(0.1f, 0);
    }

    public void ClickRankFriend()
    {
        texts[1].DOFade(1, 0);
        texts[0].DOFade(0.1f, 0);
    }
}
