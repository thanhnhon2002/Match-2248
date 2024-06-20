using System.Numerics;
using UnityEngine.UI;

public class SetTextStartFrom : SetTextPanel
{
    public override void SetText()
    {
        base.SetText();
        texts[0].text = "1024";
        texts[0].GetComponentInParent<Button>(true).image.color = CellColor.Instance.GetCellColor(1024);
        texts[0].color = CellColor.Instance.GetTextColor(1024);
        texts[1].text = "2048";
        texts[1].GetComponentInParent<Button>(true).image.color = CellColor.Instance.GetCellColor(2048);
        texts[1].color = CellColor.Instance.GetTextColor(2048);
        texts[2].text = "4096";
        texts[2].GetComponentInParent<Button>(true).image.color = CellColor.Instance.GetCellColor(4096);
        texts[2].color = CellColor.Instance.GetTextColor(4096);

        var lastValue = BigInteger.Pow(2, GameSystem.userdata.lastHighestCellValue);
        texts[3].transform.parent.gameObject.SetActive(GameSystem.userdata.lastHighestCellValue > 12);
        texts[3].text = BigIntegerConverter.ConvertNameValue(lastValue);
        texts[3].GetComponentInParent<Button>(true).image.color = CellColor.Instance.GetCellColor(lastValue);
        texts[3].color = CellColor.Instance.GetTextColor(lastValue);
    }
}