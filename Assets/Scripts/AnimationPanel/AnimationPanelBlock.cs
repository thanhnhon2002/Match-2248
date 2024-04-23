using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using DarkcupGames;
using Unity.VisualScripting;

public class AnimationPanelBlock : AnimationPanel
{
    Image[] images = new Image[3];
    public Image crown;
    public Image close;
    public override void Awake()
    {
        base.Awake();
        int index=0;
        foreach (Transform child in transform)
        {
            images[index] = child.GetComponent<Image>();
            index++;
        }
    }
    
    public override void Animation()
    {
        base.Animation();
        rectTransform.anchoredPosition = new Vector2(340, rectTransform.anchoredPosition.y);
        ///
        crown?.transform.SetParent(images[0].transform);
        if(crown!=null) crown.GetComponent<RectTransform>().anchoredPosition = Vector2.zero;
        ///
        close?.gameObject.SetActive(false);
        ///
        sequence.AppendCallback(() =>
        {
            rectTransform.DOAnchorPosX(125, 0.3f);
            images[0].rectTransform.DOSizeDelta(new Vector2(235, 235), 0.25f);
        });
        sequence.AppendInterval(0.65f);
        sequence.AppendCallback(()=>
        {
            ///
            crown?.transform.SetParent(images[1].transform);
            if (crown != null)
            {
                EasyEffect.Appear(crown.gameObject, 0.5f, 1, 0.2f);
                crown.GetComponent<RectTransform>().anchoredPosition = Vector2.zero;
            }
            ///
            if (close != null) EasyEffect.Appear(close.gameObject, 0.5f, 1, 0.2f);
            ///
            rectTransform.DOAnchorPosX(0, 0.3f);
            images[0].rectTransform.DOSizeDelta(new Vector2(175, 175), 0.25f);
            images[1].rectTransform.DOSizeDelta(new Vector2(235, 235), 0.25f);
        });
    }
    protected override void OnDisable()
    {
        rectTransform.anchoredPosition = new Vector2(340, rectTransform.anchoredPosition.y);
        images[0].rectTransform.sizeDelta = new Vector2(175, 175);
        images[1].rectTransform.sizeDelta = new Vector2(175, 175);
        images[2].rectTransform.sizeDelta = new Vector2(175, 175);
    }
}
