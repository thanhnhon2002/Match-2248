using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using DarkcupGames;

public class CellPartical : MonoBehaviour
{
    [SerializeField] private Vector3 localPos;
    [SerializeField] private float size;
    [SerializeField] private float rotation;
    public SpriteRenderer spriteRenderer;
    private void Awake()
    {
        localPos = transform.localPosition;
        size = transform.localScale.x;
        rotation = transform.eulerAngles.z;
    }

    public void PlayEffectOut (float time, Color startColor)
    {
       // AudioSystem.Instance.PlaySound("QT_paopao");
        startColor.a = 0f;
        spriteRenderer.color = startColor;
        transform.localPosition = Vector3.zero;
        transform.localScale = Vector3.zero;
        transform.localRotation = new Quaternion (0, 0, 0, 0);
        spriteRenderer.DOFade (1f, time);
        transform.DOLocalMove (localPos, time);
        transform.DORotate (new Vector3 (0, 0, rotation), time);
        transform.DOScale (size, time);
    }

    public void PlayEffectIn(float time, Color endColor)
    {
        transform.DOScale (0, time);
        transform.DOLocalMove (Vector3.zero, time);
        transform.DOLocalRotate (Vector3.zero, time);
        spriteRenderer.DOFade (0f, time);
        spriteRenderer.DOColor (endColor, time);
    }

    [ContextMenu ("Get Info")]

    private void GetInfo ()
    {
        localPos = transform.localPosition;
        size = transform.localScale.x;
        rotation = transform.eulerAngles.z;
    }
}
