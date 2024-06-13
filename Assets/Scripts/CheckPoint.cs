using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CheckPoint : MonoBehaviour
{
    [SerializeField] private CanvasGroup group;
    [SerializeField] private TextMeshProUGUI textMesh;
    [SerializeField] private Mask mask;
    [SerializeField] private Image maskImg;
    public BigInteger dislayValue;

    private void OnEnable()
    {
        group.alpha = 0f;
    }

    public void Display(BigInteger value)
    {
        dislayValue = value;
        mask.enabled = dislayValue <= GameSystem.userdata.highestCellValue;
        maskImg.gameObject.SetActive(mask.enabled);
        group.alpha = 0f;
        textMesh.text = BigIntegerConverter.ConverNameValue(value);
        group.DOFade(1f, 0.2f);
    }
}
