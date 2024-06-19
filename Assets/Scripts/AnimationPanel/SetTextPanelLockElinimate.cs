using UnityEngine;
using System.Numerics;
using UnityEngine.UI;

public class SetTextPanelLockElinimate : SetTextPanel
{
    public override void SetText()
    {
        base.SetText();
        if (GridManager.Instance.MinIndex - 2 == 0)

        {
            texts[0].text = "0";
            texts[0].GetComponentInParent<Image>(true).color = CellColor.Instance.GetCellColor((BigInteger)Mathf.Pow(2, 5));
        }
        else
        {
            texts[0].text = BigIntegerConverter.ConvertNameValue((BigInteger)Mathf.Pow(2, GridManager.Instance.MinIndex - 2));
            texts[0].GetComponentInParent<Image>(true).color = CellColor.Instance.GetCellColor((BigInteger)Mathf.Pow(2, GridManager.Instance.MinIndex - 2));
        }

        texts[1].text = BigIntegerConverter.ConvertNameValue((BigInteger)Mathf.Pow(2,GridManager.Instance.MinIndex-1));
        texts[1].GetComponentInParent<Image>(true).color = CellColor.Instance.GetCellColor((BigInteger)Mathf.Pow(2, GridManager.Instance.MinIndex - 1));

        texts[2].text = BigIntegerConverter.ConvertNameValue((BigInteger)Mathf.Pow(2,GridManager.Instance.MinIndex));
        texts[2].GetComponentInParent<Image>(true).color = CellColor.Instance.GetCellColor((BigInteger)Mathf.Pow(2, GridManager.Instance.MinIndex));

    }
}
