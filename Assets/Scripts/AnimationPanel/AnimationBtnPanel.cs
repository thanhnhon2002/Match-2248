using System;
using System.Numerics;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class AnimationBtnPanel : MonoBehaviour
{
    [SerializeField] private float cost;
    [SerializeField] private ClaimStartFrom claimButton;
    [SerializeField] private bool isAd;
    private TextMeshProUGUI textMesh;
    public FollowTarget boder;
    Button button;
    private void Awake()
    {
        textMesh = GetComponentInChildren<TextMeshProUGUI>();
        button = GetComponent<Button>();
        button.onClick.AddListener(OnClickBtn);
    }

    private void OnEnable()
    {
        if(transform.GetSiblingIndex() == 0)
        {
            button.onClick.Invoke();
        }
    }

    public void OnClickBtn()
    {
        boder.target = transform;
        claimButton.Cost = cost;
        //BigInteger.TryParse(textMesh.text, out var value);
        //Mathf math;
        //GridManager.Instance.SetIndexChose(math.LogBigInt(value,2));
        claimButton.IsAd = isAd;
    }
}