using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using TMPro;
using UnityEngine;

public class PersonalRank : MonoBehaviour
{
    [SerializeField] private RankDisplay rankDisplay;
    [SerializeField] private TextMeshProUGUI rank;
    [SerializeField] private TextMeshProUGUI score;

    private void OnEnable()
    {
        if (rankDisplay.UserDataServers == null || rankDisplay.UserDataServers.Count == 0) return;
        DisplayPersonalRank();
    }

    public void DisplayPersonalRank()
    {
        rank.text = string.Empty;
        score.text = BigIntegerConverter.ConvertNameValue(ServerSystem.user.hightScore);
        var place = rankDisplay.UserDataServers.Find(x => x.id.Equals(ServerSystem.user.id));
        if (place == null) rank.text = $"{rankDisplay.UserDataServers.Count}+";
        else rank.text = (rankDisplay.UserDataServers.IndexOf(place) + 1).ToString();
    }
}
