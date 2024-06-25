using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class PlayerInfo : MonoBehaviour
{
    [SerializeField] private Sprite[] avatarSprites;
    [SerializeField] private CanvasGroup group;
    [SerializeField] private Image avatar;
    [SerializeField] private TextMeshProUGUI nameTxt;
    [SerializeField] private TextMeshProUGUI bestScoreTxt;
    [SerializeField] private TextMeshProUGUI id;
    [SerializeField] private TextMeshProUGUI rank;
    [SerializeField] private PopupChangeName changeName;

    private void OnEnable ()
    {
        changeName.gameObject.SetActive(false);
        DisplayInfo();
    }

    public async void DisplayInfo()
    {
        var userData = GameSystem.userdata;
        if (ServerSystem.user.typeLogin == UserDataServer.TypeLogin.Guest) avatar.sprite = avatarSprites[userData.avatarIndex];
        else avatar.sprite = await Avatar.LoadAvatar(ServerSystem.user.avatarPath);
        nameTxt.text = GetUserName();
        id.text = ServerSystem.user.id;
        rank.text = string.Empty;
        LeanTween.value(0f, (float)userData.highestScore, Const.DEFAULT_TWEEN_TIME).setOnUpdate(x =>
        {
            bestScoreTxt.text = ((int)x).ToString();
        });
        await DataRankManager.GetRankGlobal(DisplayRank, null);
    }

    public void DisplayRank(List<UserDataServer> users)
    {
        rank.text = string.Empty;
        var place = users.Find(x => x.id.Equals(ServerSystem.user.id));
        if (place == null) rank.text = $"{users.Count}+";
        else rank.text = (users.IndexOf(place) + 1).ToString();
    }

    private string GetUserName()
    {
        var user = ServerSystem.user;
        return user.typeLogin switch
        {
            UserDataServer.TypeLogin.Google => GoogleAuthentication.Name,
            UserDataServer.TypeLogin.Facebook => FacebookAuthentication.Name,
            _ => GameSystem.userdata.nickName
        };;
    }
}
