using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SentFriendRequestInfo : MonoBehaviour
{
    [SerializeField] private Image avatar;
    [SerializeField] private TextMeshProUGUI userName;
    [SerializeField] private TextMeshProUGUI id;
    [SerializeField] private Button BtnRemoveRequest;

    public async Task DisplayInfo(UserDataServer data)
    {
        if (data.id == null) return;
        userName.text = data.nickName;
        id.text = data.id;
        BtnRemoveRequest.onClick.AddListener(() => { OnClickAcceptFriend(data.id); });
        if (data.typeLogin == UserDataServer.TypeLogin.Guest) avatar.sprite = AvatarManager.Instance.avatars[data.avatarIndex];
        else avatar.sprite = await Avatar.LoadAvatar(data.avatarPath);
    }

    private void OnClickAcceptFriend(string id)
    {
        DataFriendManager.RemoveFriend(id);
        BtnRemoveRequest.onClick.RemoveAllListeners();
    }
}
