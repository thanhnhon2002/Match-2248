using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ConectedPointDisplay : MonoBehaviour
{
    [SerializeField] private Image image;
    [SerializeField] private TextMeshProUGUI valueTtx;

    private void Awake ()
    {
        image = GetComponent<Image> ();
    }

    public void Show(BigInteger point)
    {
        gameObject.SetActive(true);
        valueTtx.text = BigIntegerConverter.ConvertNameValue (point);
        valueTtx.color = CellColor.Instance.GetTextColor(point);
        image.color = CellColor.Instance.GetCellColor(point);
    }
}
