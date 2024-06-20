using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum OptionMenu
{
    Home,
    PlayerInformation,
    Shop,
}
public class MenuOptions : MonoBehaviour
{
    public static MenuOptions Instance;
    Dictionary<OptionMenu, GameObject> dicMenuOptions = new Dictionary<OptionMenu, GameObject>();
    [SerializeField] OptionAnimation defaultOption;
    [SerializeField] GameObject panel;
    [SerializeField] Vector3 normalPos = new Vector3(1100, 250, 0);
    Sequence sequence;
    private void Awake()
    {
        var options = GetComponentsInChildren<Option>(true);
        foreach (var option in options) dicMenuOptions.Add(option.option, option.gameObject);
        Instance = this;
    }
    public void Start()
    {
        panel.GetComponent<RectTransform>().localPosition = normalPos;
        if (GameSystem.userdata.firstPlayGame) return;
        OptionAnimation.optionAnimation = defaultOption;
        ShowOption(OptionAnimation.optionAnimation.option);
        OnPanel();
    }
    public void ShowOption(OptionMenu option)
    {
        dicMenuOptions[option].SetActive(true);
    }
    public void HideOption(OptionMenu option)
    {
        dicMenuOptions[option].SetActive(false);
    }
    public void HideAllOption(OptionMenu optionException)
    {
        foreach (var option in dicMenuOptions.Keys)
            if (option != optionException)
            {
                if (dicMenuOptions[option].activeInHierarchy) dicMenuOptions[option].SetActive(false);
            }
            else if (!dicMenuOptions[option].activeInHierarchy) dicMenuOptions[option].SetActive(true);
    }
    void OnPanel()
    {       
        panel.GetComponent<RectTransform>().anchoredPosition = normalPos;
        panel.GetComponent<RectTransform>().DOAnchorPosX(normalPos.x, 0.5f).OnComplete(()=>
        {
            panel.GetComponent<RectTransform>().DOAnchorPosX(0, 0.3f).OnComplete(()=>
            {
                AnimationPanel();
            });
        });
    }
    void AnimationPanel()
    {
        sequence = DOTween.Sequence();
        var optionAnimations = panel.GetComponentsInChildren<OptionAnimation>();
        sequence.AppendCallback(() =>
        {
            optionAnimations[0].AnimationUp(0.008f);
            optionAnimations[4].AnimationUp(0.008f);
        }).AppendInterval(0.4f);
        sequence.AppendCallback(() =>
        {
            optionAnimations[0].AnimationDown();
            optionAnimations[4].AnimationDown();
            optionAnimations[1].AnimationUp(0.008f);
            optionAnimations[3].AnimationUp(0.008f);
        }).AppendInterval(0.4f);
        sequence.AppendCallback(() =>
        {
            optionAnimations[1].AnimationDown();
            optionAnimations[3].AnimationDown();
            OptionAnimation.optionAnimation.AnimationUp(0.005f);
        });
    }
    private void OnDestroy()
    {
        sequence.Kill();
        DOTween.Kill(panel.GetComponent<RectTransform>());
        panel.GetComponent<RectTransform>().localPosition = normalPos;
    }
}
