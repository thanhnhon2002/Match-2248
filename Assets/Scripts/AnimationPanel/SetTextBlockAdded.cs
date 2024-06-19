using UnityEngine;
using System.Numerics;
using System;
using UnityEngine.UI;

public class SetTextBlockAdded : SetTextPanel
{
    public override void SetText()
    {
        base.SetText();
        texts[0].text = BigIntegerConverter.ConvertNameValue((BigInteger)Mathf.Pow(2, GridManager.Instance.MaxIndexRandom - 1));
        texts[0].GetComponentInParent<Image>(true).color = CellColor.Instance.GetCellColor((BigInteger)Mathf.Pow(2, GridManager.Instance.MaxIndexRandom - 1));

        texts[1].text = BigIntegerConverter.ConvertNameValue((BigInteger)Mathf.Pow(2, GridManager.Instance.MaxIndexRandom));
        texts[1].GetComponentInParent<Image>(true).color = CellColor.Instance.GetCellColor((BigInteger)Mathf.Pow(2, GridManager.Instance.MaxIndexRandom ));

        texts[2].text = BigIntegerConverter.ConvertNameValue((BigInteger)Mathf.Pow(2, GridManager.Instance.MaxIndexRandom + 1));
        texts[2].GetComponentInParent<Image>(true).color = CellColor.Instance.GetCellColor((BigInteger)Mathf.Pow(2, GridManager.Instance.MaxIndexRandom + 1));

    }
}
