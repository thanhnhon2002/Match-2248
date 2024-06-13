using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BestCell : MonoBehaviour
{
    [SerializeField] private Image cellImg;
    [SerializeField] private TextMeshProUGUI valueTxt;

    private void Start ()
    {
        DisplayBestCell();
    }

    public void DisplayBestCell()
    {
        var userData = GameSystem.userdata;
        var value = userData.gameData.currentHighestCellValue;
        if (value < 2) value = 2;
        cellImg.color = CellColor.Instance.GetCellColor(value);
        valueTxt.text = BigIntegerConverter.ConverNameValue(value);
    }
}
