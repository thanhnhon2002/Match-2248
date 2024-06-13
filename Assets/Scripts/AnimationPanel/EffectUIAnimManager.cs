
using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectUIAnimManager : MonoBehaviour
{
    public float dotweenSpeed;
    public Transform targetTransform;

    private Tweener tweener;
    private void OnEnable()
    {
        RandomRotate();
        Scale();
    }

    private void Scale()
    {
        tweener = transform.DOScale(1.15f, dotweenSpeed).SetEase(Ease.OutBack);
    }

    public void MoveToTarget()
    {
        tweener.Kill();
        transform.DOMove(targetTransform.transform.position, dotweenSpeed).SetEase(Ease.InBack);
        transform.DOScale(0f, dotweenSpeed).OnComplete(() =>
        {
            Destroy(gameObject);
        });
    }

    private void RandomRotate()
    {
        transform.DORotate(new Vector3(0,0,UnityEngine.Random.Range(0,360)),0);
    }

    private void OnDisable()
    {
        tweener.Kill();
    }
}
