using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class RewardMultiplier : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI multiplierTxt;
    [SerializeField] private int multiplierDiamond = 1;
    public int MultiplierDiamond => multiplierDiamond;
    private void OnEnable()
    {
        if (multiplierDiamond == 1) multiplierTxt.text = "";
        else multiplierTxt.text = $"x{multiplierDiamond}";
    }
}
