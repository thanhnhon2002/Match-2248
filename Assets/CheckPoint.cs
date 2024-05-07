using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using TMPro;
using UnityEngine;

public class CheckPoint : MonoBehaviour
{
    [SerializeField] private CanvasGroup group;
    [SerializeField] private TextMeshProUGUI textMesh;
    public BigInteger dislayValue;

    private void OnEnable()
    {
        group.alpha = 0f;
    }

    public void Display(BigInteger value)
    {
        dislayValue = value;
        group.alpha = 0f;
        textMesh.text = BigIntegerConverter.ConverNameValue(value);
        group.DOFade(1f, 0.2f);
    }
}
