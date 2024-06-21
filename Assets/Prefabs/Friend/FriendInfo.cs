using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System.Threading.Tasks;

public class FriendInfo : MonoBehaviour
{
    [SerializeField] private Image avatar;
    [SerializeField] private TextMeshProUGUI userName;
    [SerializeField] private TextMeshProUGUI id;
    [SerializeField] private Button BtnAddFriend;

    public async Task DisplayInfo(UserDataServer data)
    {
        if (data.id == null) return;
        userName.text = data.nickName;
        id.text = data.GetID();
        BtnAddFriend.onClick.AddListener(() => { OnCikcBtnAddFriend(data.id); });
        if (data.typeLogin == UserDataServer.TypeLogin.Guest) avatar.sprite = AvatarManager.Instance.avatars[data.avatarIndex];
        else avatar.sprite = await Avatar.LoadAvatar(data.avatarPath, avatar.rectTransform.rect, avatar.rectTransform.pivot);
    }

    public async Task DisplayInfoRequest(UserDataServer data)
    {
        if (data.id == null) return;
        userName.text = data.nickName;
        id.text = data.GetID();
        BtnAddFriend.onClick.AddListener(() => { OnClickAcceptFriend(data.id); });
        if (data.typeLogin == UserDataServer.TypeLogin.Guest) avatar.sprite = AvatarManager.Instance.avatars[data.avatarIndex];
        else avatar.sprite = await Avatar.LoadAvatar(data.avatarPath, avatar.rectTransform.rect, avatar.rectTransform.pivot);
    }
    private void OnCikcBtnAddFriend(string id)
    {
        DataFriendManager.AddFriend(id);
        BtnAddFriend.onClick.RemoveAllListeners();
    }

    private void OnClickAcceptFriend(string id)
    {
        DataFriendManager.AcceptFriend(id);
        BtnAddFriend.onClick.RemoveAllListeners();
    }
}
