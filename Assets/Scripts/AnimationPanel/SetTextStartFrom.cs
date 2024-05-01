using UnityEngine.UI;

public class SetTextStartFrom : SetTextPanel
{
    public override void SetText()
    {
        base.SetText();
        texts[0].text = "2";
        texts[0].GetComponentInParent<Image>(true).color = CellColor.Instance.GetCellColor(2);
        texts[1].text = "1024";
        texts[1].GetComponentInParent<Image>(true).color = CellColor.Instance.GetCellColor(1024);
        texts[2].text = "2048";
        texts[2].GetComponentInParent<Image>(true).color = CellColor.Instance.GetCellColor(2048);
        texts[3].text = "4096";
        texts[3].GetComponentInParent<Image>(true).color = CellColor.Instance.GetCellColor(4096);

    }
}