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
        userName.text = data.nickName;
        score.text = BigIntegerConverter.ConvertNameValue(BigInteger.Pow(2, data.indexPlayer));
        id.text = data.id;
        if (data.typeLogin == UserDataServer.TypeLogin.Guest)
        {
            if(rankDisplay == null ) rankDisplay = GetComponentInParent<RankDisplay>();
            avatar.sprite = rankDisplay.avatar[data.avatarIndex];
        } else avatar.sprite = await Avatar.LoadAvatar(data.avatarPath);
    }
}
