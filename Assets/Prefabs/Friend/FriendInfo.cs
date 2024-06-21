using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System.Threading.Tasks;

public class FriendInfo : MonoBehaviour
{
    private RankDisplay rankDisplay;
    [SerializeField] private Image avatar;
    [SerializeField] private TextMeshProUGUI userName;
    [SerializeField] private TextMeshProUGUI id;
    [SerializeField] private Button BtnAddFriend;

    private void Awake()
    {
        rankDisplay = GetComponentInParent<RankDisplay>();
    }

    public async Task DisplayInfo(UserDataServer data)
    {
        userName.text = data.nickName;
        id.text = data.GetID();
        BtnAddFriend.onClick.AddListener(() => { OnCikcBtnAddFriend(data.id); });
        if (data.typeLogin == UserDataServer.TypeLogin.Guest) avatar.sprite = rankDisplay.avatar[data.avatarIndex];
        else avatar.sprite = await Avatar.LoadAvatar(data.avatarPath, avatar.rectTransform.rect, avatar.rectTransform.pivot);
    }
    private void OnCikcBtnAddFriend(string id)
    {

    }
}
