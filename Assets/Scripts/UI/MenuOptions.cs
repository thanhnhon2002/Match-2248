using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum OptionMenu
{
    Home,
    PlayerInformation,
    Shop,
    Rank,
    Friend
}
public class MenuOptions : MonoBehaviour
{
    public const float ANIMATION_MOVE_TIME = 0.008f;
    public const float ANIMATION_MOVE_TIME_SLOW = 0.005f;
    public const bool ANIMATION_ALL_PANEL = false;

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

    public void ShowDefaultOption()
    {
        OptionAnimation.optionAnimation.AnimationDown();
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
        if (ANIMATION_ALL_PANEL)
        {
            panel.GetComponent<RectTransform>().anchoredPosition = normalPos;
            panel.GetComponent<RectTransform>().DOAnchorPosX(normalPos.x, 0.5f).OnComplete(() =>
            {
                panel.GetComponent<RectTransform>().DOAnchorPosX(0, 0.3f).OnComplete(() =>
                {
                    AnimationPanel();
                });
            });
        } else
        {
            panel.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, normalPos.y);
            OptionAnimation.optionAnimation.AnimationUp(ANIMATION_MOVE_TIME_SLOW);
        }
    }
    void AnimationPanel()
    {
        sequence = DOTween.Sequence();
        var optionAnimations = panel.GetComponentsInChildren<OptionAnimation>();
        sequence.AppendCallback(() =>
        {
            optionAnimations[0].AnimationUp(ANIMATION_MOVE_TIME);
            optionAnimations[4].AnimationUp(ANIMATION_MOVE_TIME);
        }).AppendInterval(0.5f);
        sequence.AppendCallback(() =>
        {
            optionAnimations[0].AnimationDown();
            optionAnimations[4].AnimationDown();
            optionAnimations[1].AnimationUp(ANIMATION_MOVE_TIME);
            optionAnimations[3].AnimationUp(ANIMATION_MOVE_TIME);
        }).AppendInterval(0.5f);
        sequence.AppendCallback(() =>
        {
            optionAnimations[1].AnimationDown();
            optionAnimations[3].AnimationDown();
            OptionAnimation.optionAnimation.AnimationUp(ANIMATION_MOVE_TIME_SLOW);
        });
    }
    private void OnDestroy()
    {
        sequence.Kill();
        DOTween.Kill(panel.GetComponent<RectTransform>());
        panel.GetComponent<RectTransform>().localPosition = normalPos;
    }
}
