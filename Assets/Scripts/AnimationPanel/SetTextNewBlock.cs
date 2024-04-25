using UnityEngine;
using System.Numerics;
using UnityEngine.UI;

public class SetTextNewBlock : SetTextPanel
{
    public override void SetText()
    {
        base.SetText();
        texts[0].text = BigIntegerConverter.ConverNameValue((BigInteger)Mathf.Pow(2, GridManager.Instance.IndexPlayer - 1));
        texts[0].GetComponentInParent<Image>(true).color = CellColor.Instance.GetCellColor((BigInteger)Mathf.Pow(2, GridManager.Instance.IndexPlayer - 1));

        texts[1].text = BigIntegerConverter.ConverNameValue((BigInteger)Mathf.Pow(2, GridManager.Instance.IndexPlayer));
        texts[1].GetComponentInParent<Image>(true).color = CellColor.Instance.GetCellColor((BigInteger)Mathf.Pow(2, GridManager.Instance.IndexPlayer));

        texts[2].text = BigIntegerConverter.ConverNameValue((BigInteger)Mathf.Pow(2, GridManager.Instance.IndexPlayer + 1));
        texts[2].GetComponentInParent<Image>(true).color = CellColor.Instance.GetCellColor((BigInteger)Mathf.Pow(2, GridManager.Instance.IndexPlayer + 1));


    }
}