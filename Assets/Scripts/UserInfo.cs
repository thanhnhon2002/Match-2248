using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System.Threading.Tasks;
using Firebase.Database;
using System;

public class UserInfo : MonoBehaviour
{
    [SerializeField] private Image avatar;
    [SerializeField] private TextMeshProUGUI userName;
    [SerializeField] private TextMeshProUGUI id;
    [SerializeField] private Button BtnAddFriend;

    public async Task DisplayInfo(UserDataServer data)
    {
        if (data.id == null) return;
        userName.text = data.nickName;
        id.text = data.id;
        BtnAddFriend.onClick.AddListener(() => { OnCikcBtnAddFriend(data.id); });
        if (data.typeLogin == UserDataServer.TypeLogin.Guest) avatar.sprite = AvatarManager.Instance.avatars[data.avatarIndex];
        else avatar.sprite = await Avatar.LoadAvatar(data.avatarPath);
    }
    private void OnCikcBtnAddFriend(string id)
    {
        DataFriendManager.AddFriend(id);
        BtnAddFriend.onClick.RemoveAllListeners();
    } 
}
