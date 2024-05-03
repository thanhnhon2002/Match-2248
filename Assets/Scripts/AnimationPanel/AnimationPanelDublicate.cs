using UnityEngine;
using DG.Tweening;

public class AnimationPanelDublicate : AnimationPanel
{
    [SerializeField] GameObject image;
    Vector3 originScalse;
    public override void Awake()
    {
        base.Awake();
        originScalse = image.transform.localScale;
    }
    protected override void OnEnable()
    {
        base.OnEnable();       
    }
    public override void Animation()
    {
        base.Animation();
        LeanTween.delayedCall(0.2f, () => {
            image.transform.localScale = new Vector3(originScalse.x * -1, originScalse.y, originScalse.z);
            image.transform.DOScaleX(originScalse.x, 1f);
        });        
    }
    protected override void OnDisable()
    {
        image.transform.localScale = originScalse;
        base.OnDisable();
    }
}
