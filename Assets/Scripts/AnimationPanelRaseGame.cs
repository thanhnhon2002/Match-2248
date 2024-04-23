using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using DarkcupGames;

public class AnimationPanelRaseGame : AnimationPanel
{
    Image[] images = new Image[10];
    protected override void Awake()
    {
        base.Awake();
        foreach (Transform child in transform) images[child.transform.GetSiblingIndex()] = child.GetComponent<Image>();
    }
    public override void Animation()
    {
        base.Animation();
        for(int i=0;i<images.Length;i++)
        {
            if (images[i].gameObject.activeInHierarchy) return;
            sequence.AppendCallback(()=>
            {
                EasyEffect.Appear(images[i].gameObject, 0.3f, 1, 0.2f);
                images[i+1].gameObject.SetActive(false);
            });
            sequence.AppendInterval(0.15f);
        }
    }
    protected override void OnDisable()
    {
        base.OnDisable();
        for (int i = 0; i < images.Length; i++)
        {
            if (images[i].gameObject.activeInHierarchy) images[i].gameObject.SetActive(false);
            else images[i].gameObject.SetActive(true);
        }
    }

}