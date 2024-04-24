using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class BonusDiamond : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI bonusTxt;

    public void DisplayBonus(int amount, Vector3 appearPosition)
    {
        bonusTxt.text = $"x{amount}";
        gameObject.SetActive (true);
    }
}
