using UnityEngine;
using System.Numerics;

public class SetTextPanelLockElinimate : SetTextPanel
{
    public override void SetText()
    {
        base.SetText();
        texts[0].text = BigIntegerConverter.ConverNameValue((BigInteger)Mathf.Pow(2,GridManager.Instance.MinIndex-2));
        texts[1].text = BigIntegerConverter.ConverNameValue((BigInteger)Mathf.Pow(2,GridManager.Instance.MinIndex-1));
        texts[2].text = BigIntegerConverter.ConverNameValue((BigInteger)Mathf.Pow(2,GridManager.Instance.MinIndex));
    }
}
