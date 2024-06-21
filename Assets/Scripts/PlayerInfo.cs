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

    public void DisplayInfo()
    {
        var userData = GameSystem.userdata;
        avatar.sprite = avatarSprites[userData.avatarIndex];
        nameTxt.text = userData.nickName;
        id.text = ServerSystem.user.GetID();
        LeanTween.value(0f, (float)userData.highestScore, Const.DEFAULT_TWEEN_TIME).setOnUpdate(x =>
        {
            bestScoreTxt.text = ((int)x).ToString();
        });
    }
}
