using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class FriendRequestInfo : MonoBehaviour
{
    [SerializeField] private Image avatar;
    [SerializeField] private TextMeshProUGUI userName;
    [SerializeField] private TextMeshProUGUI id;
    [SerializeField] private Button BtnAddFriend;
    [SerializeField] private Button BtnRemoveFriend;

    public async Task DisplayInfo(UserDataServer data)
    {
        if (data.id == null) return;
        userName.text = data.nickName;
        id.text = data.id;
        BtnAddFriend.onClick.AddListener(() => { OnClickAcceptFriend(data.id); });
        BtnRemoveFriend.onClick.AddListener(() => { OncikBtnRemoveFriend(data.id); });
        if (data.typeLogin == UserDataServer.TypeLogin.Guest) avatar.sprite = AvatarManager.Instance.avatars[data.avatarIndex];
        else avatar.sprite = await Avatar.LoadAvatar(data.avatarPath);
    }

    private void OnClickAcceptFriend(string id)
    {
        DataFriendManager.AcceptFriend(id);
        BtnAddFriend.onClick.RemoveAllListeners();
    }

    private void OncikBtnRemoveFriend(string id)
    {
        DataFriendManager.RemoveFriend(id);
        BtnRemoveFriend.onClick.RemoveAllListeners();
    }
}
