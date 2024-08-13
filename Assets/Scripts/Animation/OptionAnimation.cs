using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using DarkcupGames;

public class OptionAnimation : MonoBehaviour,IPointerClickHandler,IPointerUpHandler,IPointerDownHandler,IPointerEnterHandler,IPointerExitHandler,IDragHandler,IEndDragHandler,IBeginDragHandler
{
    [SerializeField] Image iconOption;
    [SerializeField] Sprite iconUsed;
    Sprite iconNormal;
    [SerializeField] Vector2 posUp;
    [SerializeField] Vector2 sizeUp;
    [SerializeField] Vector2 sizeDown;
    const float TIME_ANIMATON_UP = 0.001f;
    const float TIME_ANIMATON_DOWN = 0.008f;
    TextMeshProUGUI nameOption;
    //Sequence sequence;
    RectTransform rectTransformImageUsed;
    public OptionMenu option;
    public static OptionAnimation optionAnimation;
    public static float lastClickTime;
    static bool draging;
    void Awake()
    {
        iconOption = transform.GetComponentsInChildren<Image>()[1];    
        rectTransformImageUsed = iconOption.GetComponent<RectTransform>();
        nameOption = GetComponentInChildren<TextMeshProUGUI>(true);
        iconNormal = iconOption.sprite;
    }
    void Start()
    {
        if (option == OptionMenu.Home) return;
        nameOption.gameObject.SetActive(false);
    }
    public void OnPointerEnter(PointerEventData eventData)
    {
        //Debug.Log("Enter "+transform.name);
        if (optionAnimation != null&&draging)
        {
            optionAnimation.AnimationDown();
            optionAnimation = this;
            AnimationUp();
        }
    }
    public void OnPointerExit(PointerEventData eventData)
    {
        //Debug.Log("Exit " + transform.name);
    }
    public void OnPointerDown(PointerEventData eventData)
    {
        //Debug.Log("Down " + transform.name);
        //Debug.Log("Click " + transform.name);
        OpenPanel();
    }
    public void OnBeginDrag(PointerEventData eventData)
    {
        //Debug.Log("Begin Drag");
        draging = true;
    }
    public void OnDrag(PointerEventData eventData)
    {
        //Debug.Log("Draging " + transform.name);
    }  
    public void OnPointerUp(PointerEventData eventData)
    {
        //Debug.Log("Up + USE " + transform.name);
        MenuOptions.Instance.HideAllOption(optionAnimation.option);
    }
    public void OnEndDrag(PointerEventData eventData)
    {
        //Debug.Log("End Drag " + transform.name);
        draging = false;
    }
    public void OnPointerClick(PointerEventData eventData)
    {
        //Debug.Log("Click " + transform.name);
        OpenPanel();
    }

    public void OpenPanel()
    {
        if (optionAnimation != null)
        {
            optionAnimation.AnimationDown();
        }
        optionAnimation = this;
        AnimationUp();
    }

    public void AnimationUp(float speed = TIME_ANIMATON_UP)
    {
        AdManagerMax.Instance.ShowIntertistial("home_tab", null);
        DestroyAniamtion();
        //sequence = DOTween.Sequence();
        iconOption.sprite = iconUsed;
        rectTransformImageUsed.DOLocalMoveY(posUp.y, speed * (posUp.y-rectTransformImageUsed.localPosition.y));
        rectTransformImageUsed.DOSizeDelta(sizeUp, speed * (sizeUp.x - rectTransformImageUsed.localPosition.x));
        nameOption.gameObject.SetActive(true);
    }
    public void AnimationDown(float speed = TIME_ANIMATON_DOWN)
    {
        DestroyAniamtion();
        //sequence = DOTween.Sequence();
        iconOption.sprite = iconNormal;
        rectTransformImageUsed.DOLocalMoveY(0, speed * rectTransformImageUsed.localPosition.y);
        rectTransformImageUsed.DOSizeDelta(sizeDown, speed * (rectTransformImageUsed.localPosition.x - sizeDown.x));
        nameOption.gameObject.SetActive(false);
    }
    void DestroyAniamtion()
    {
        //sequence.Kill();
        DOTween.Kill(rectTransformImageUsed);
        DOTween.Kill(iconOption);
    }
    void OnDestroy()
    {
        DestroyAniamtion();
    }
    
}
