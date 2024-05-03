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

    private void OnEnable ()
    {
        var userData = GameSystem.userdata;
        avatar.sprite = avatarSprites[userData.avatarIndex];
        nameTxt.text = userData.nickName;
        LeanTween.value (0f, (float)userData.highestScore, Const.DEFAULT_TWEEN_TIME).setOnUpdate (x =>
        {
            bestScoreTxt.text = ((int)x).ToString();
        });
    }

    public void Close()
    {
        transform.DOScale (0f, Const.DEFAULT_TWEEN_TIME);
        group.DOFade (0f, Const.DEFAULT_TWEEN_TIME).OnComplete (() => gameObject.SetActive (false));
    }
}
