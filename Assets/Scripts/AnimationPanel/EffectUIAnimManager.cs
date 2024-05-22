
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
        ScaleAndMoveToTarget();
    }

    private void ScaleAndMoveToTarget()
    {
        tweener = transform.DOScale(1, dotweenSpeed).OnComplete(() =>
        {
            tweener = transform.DOMove(targetTransform.transform.position, dotweenSpeed).OnComplete(() =>
            {
                Destroy(gameObject);
            });
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
