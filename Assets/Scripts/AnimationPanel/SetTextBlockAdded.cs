using UnityEngine;
using System.Numerics;
using System;

public class SetTextBlockAdded : SetTextPanel
{
    public override void SetText()
    {
        base.SetText();
        texts[0].text = BigIntegerConverter.ConverNameValue((BigInteger)Mathf.Pow(2, GridManager.Instance.MaxIndexRandom - 1));
        texts[1].text = BigIntegerConverter.ConverNameValue((BigInteger)Mathf.Pow(2, GridManager.Instance.MaxIndexRandom));
        texts[2].text = BigIntegerConverter.ConverNameValue((BigInteger)Mathf.Pow(2, GridManager.Instance.MaxIndexRandom + 1));
    }
}
