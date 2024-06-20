using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class OptionAnimation : MonoBehaviour,IPointerClickHandler,IPointerUpHandler,IPointerDownHandler,IPointerEnterHandler,IPointerExitHandler,IDragHandler,IEndDragHandler,IBeginDragHandler
{
    [SerializeField] Image imageNormal;
    [SerializeField] Image imageUsed;
    [SerializeField] Vector2 posUp;
    [SerializeField] Vector2 sizeUp;
    [SerializeField] Vector2 sizeDown;
    const float TIME_ANIMATON_UP = 0.001f;
    const float TIME_ANIMATON_DOWN = 0.008f;
    TextMeshProUGUI nameOption;
    //Sequence sequence;
    RectTransform rectTransform;
    public OptionMenu option;
    public static OptionAnimation optionAnimation;
    static bool draging;
    void Awake()
    {
        imageUsed = transform.GetComponentsInChildren<Image>()[1];
        rectTransform = imageUsed.GetComponent<RectTransform>();
        nameOption = GetComponentInChildren<TextMeshProUGUI>();
    }
    public void OnPointerEnter(PointerEventData eventData)
    {
        Debug.Log("Enter "+transform.name);
        if (optionAnimation != null&&draging)
        {
            optionAnimation = this;
            AnimationUp();
        }
    }
    public void OnPointerExit(PointerEventData eventData)
    {
        Debug.Log("Exit " + transform.name);
        if(optionAnimation!=null && draging) AnimationDown();
    }
    public void OnPointerDown(PointerEventData eventData)
    {
        Debug.Log("Down " + transform.name);
        if (optionAnimation != null)
        {
            //if(option!=optionAnimation.option) MenuOptions.Instance.HideOption(optionAnimation.option);
            optionAnimation.AnimationDown();
        }
        optionAnimation = this;
        AnimationUp();
    }
    public void OnBeginDrag(PointerEventData eventData)
    {
        Debug.Log("Begin Drag");
        draging = true;
    }
    public void OnDrag(PointerEventData eventData)
    {
        //Debug.Log("Draging " + transform.name);
    }  
    public void OnPointerUp(PointerEventData eventData)
    {
        Debug.Log("Up + USE " + transform.name);
        MenuOptions.Instance.HideAllOption(optionAnimation.option);
        //optionAnimation = null;
    }
    public void OnEndDrag(PointerEventData eventData)
    {
        Debug.Log("End Drag " + transform.name);
        draging = false;
    }
    public void OnPointerClick(PointerEventData eventData)
    {
        Debug.Log("Click " + transform.name);
        if (optionAnimation != null)
        {
            //MenuOptions.Instance.HideOption(optionAnimation.option);
            optionAnimation.AnimationDown();
        }
        //MenuOptions.Instance.ShowOption(option);
        optionAnimation = this;
        AnimationUp();
    }
    public void AnimationUp(float speed = TIME_ANIMATON_UP)
    {
        DestroyAniamtion();
        //sequence = DOTween.Sequence();
        rectTransform.DOLocalMoveY(posUp.y, speed * (posUp.y-rectTransform.localPosition.y));
        rectTransform.DOSizeDelta(sizeUp, speed * (sizeUp.x - rectTransform.localPosition.x));
    }
    public void AnimationDown(float speed = TIME_ANIMATON_DOWN)
    {
        DestroyAniamtion();
        //sequence = DOTween.Sequence();
        rectTransform.DOLocalMoveY(0, speed * rectTransform.localPosition.y);
        rectTransform.DOSizeDelta(sizeDown, speed * (rectTransform.localPosition.x-sizeDown.x));
    }
    void DestroyAniamtion()
    {
        //sequence.Kill();
        DOTween.Kill(rectTransform);
    }
    void OnDestroy()
    {
        DestroyAniamtion();
    }
    
}
