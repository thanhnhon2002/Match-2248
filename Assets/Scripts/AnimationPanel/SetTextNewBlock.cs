using UnityEngine;
using System.Numerics;

public class SetTextNewBlock : SetTextPanel
{
    public override void SetText()
    {
        base.SetText();
        texts[0].text = BigIntegerConverter.ConverNameValue((BigInteger)Mathf.Pow(2, GridManager.Instance.IndexPlayer - 1));
        texts[1].text = BigIntegerConverter.ConverNameValue((BigInteger)Mathf.Pow(2, GridManager.Instance.IndexPlayer));
        texts[2].text = BigIntegerConverter.ConverNameValue((BigInteger)Mathf.Pow(2, GridManager.Instance.IndexPlayer + 1));

    }
}