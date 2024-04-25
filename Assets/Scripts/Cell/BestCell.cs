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
        var userData = GameSystem.userdata;
        var value = userData.highestCellValue;
        if(value < 2) value = 2;
        cellImg.color = CellColor.Instance.GetCellColor (value);
        valueTxt.text = BigIntegerConverter.ConverNameValue(value);
    }
}
