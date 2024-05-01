using UnityEngine;
using DG.Tweening;

public class AnimationPanelDublicate : AnimationPanel
{
    [SerializeField] GameObject image;
    Vector3 originScalse;
    protected override void OnEnable()
    {
        originScalse = image.transform.localScale;
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
        base.OnDisable();
    }
}
