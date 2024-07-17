using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using System.Numerics;

public class RankUserInfo : MonoBehaviour
{
    private RankDisplay rankDisplay;
    [SerializeField] private Image avatar;
    [SerializeField] private TextMeshProUGUI userName;
    [SerializeField] private TextMeshProUGUI id;
    [SerializeField] private TextMeshProUGUI score;
    [SerializeField] private TextMeshProUGUI stt;
    [SerializeField] private Image bg;

    private void Awake()
    {
        rankDisplay = GetComponentInParent<RankDisplay>();
    }

    private void OnEnable()
    {
        stt.text = (transform.GetSiblingIndex() + 1).ToString();
    }

    public async Task DisplayInfo(UserDataServer data)
    {
        bg.gameObject.SetActive(data.id.Equals(ServerSystem.user.id));
        userName.text = data.nickName;
        score.text = BigIntegerConverter.ConvertNameValue(data.hightScore);
        id.text = data.id;
        if (data.typeLogin == UserDataServer.TypeLogin.Guest)
        {
            if(rankDisplay == null ) rankDisplay = GetComponentInParent<RankDisplay>();
            avatar.sprite = rankDisplay.avatar[data.avatarIndex];
        } else avatar.sprite = await Avatar.LoadAvatar(data.avatarPath);
    }
}
