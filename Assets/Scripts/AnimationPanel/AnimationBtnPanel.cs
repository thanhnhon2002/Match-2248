using System;
using System.Numerics;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class AnimationBtnPanel : MonoBehaviour
{
    [SerializeField] private float cost;
    [SerializeField] private ClaimStartFrom claimButton;
    private TextMeshProUGUI textMesh;
    public GameObject boder;
    Button button;
    private void Awake()
    {
        textMesh = GetComponentInChildren<TextMeshProUGUI>();
        button = GetComponent<Button>();
        button.onClick.AddListener(OnClickBtn);
    }
    public void OnClickBtn()
    {
        boder.transform.position = transform.position;
        claimButton.cost = cost;
        BigInteger.TryParse(textMesh.text, out var value);
        Mathf math;
        GridManager.Instance.SetIndexChose(math.LogBigInt(value,2));
    }
}