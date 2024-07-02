using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class AdBreak : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI num1;
    [SerializeField] private TextMeshProUGUI num2;
    [SerializeField] private TextMeshProUGUI sum;
    [SerializeField] private TextMeshProUGUI diamond;
    public int diamondAmount;
    private void OnEnable()
    {
        var num1 = Random.Range(1, 300);
        var num2 = Random.Range(1, 300);
        var sum = num1 + num2;
        this.num1.text = num1.ToString();
        this.num2.text = num2.ToString();
        this.sum.text = sum.ToString();

        diamondAmount = Random.Range(5, 7);
        diamond.text = "x " + diamondAmount.ToString();
    }
}
