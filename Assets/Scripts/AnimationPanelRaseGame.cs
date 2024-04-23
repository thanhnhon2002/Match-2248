using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using DarkcupGames;

public class AnimationPanelRaseGame : AnimationPanel
{
    public Image[] images = new Image[10];
    public override void Awake()
    {
        base.Awake();
        foreach (Transform child in transform) images[child.transform.GetSiblingIndex()] = child.GetComponent<Image>();
    }
    public override void Animation()
    {
        base.Animation();
        AppendAnimation(0);
    }
    void AppendAnimation(int i)
    {
        sequence.AppendCallback(() =>
        {
            EasyEffect.Appear(images[i].gameObject, 0.3f, 1, 0.2f);
            images[i + 1].gameObject.SetActive(false);
        });
        sequence.AppendInterval(0.15f);
        if (i+2 < images.Length) AppendAnimation(i + 2);
    }
    protected override void OnDisable()
    {
        base.OnDisable();
        for (int i = 0; i < images.Length; i+=2)
        {
            images[i].gameObject.SetActive(false);
            images[i+1].gameObject.SetActive(true);
        }
    }

}