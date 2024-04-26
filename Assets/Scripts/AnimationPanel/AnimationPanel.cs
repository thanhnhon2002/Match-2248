using UnityEngine;
using DG.Tweening;

public abstract class AnimationPanel : MonoBehaviour
{
    protected Sequence sequence;
    [SerializeField] protected RectTransform rectTransform;

    public virtual void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        sequence = DOTween.Sequence();
    }
    protected virtual void OnEnable()
    {
        Animation();
    }
    public virtual void Animation() { sequence = DOTween.Sequence(); }
    protected virtual void OnDisable() { }
}
