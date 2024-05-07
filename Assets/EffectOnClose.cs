using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectOnClose : MonoBehaviour
{
    public void Close()
    {
        transform.DOScale(0f, Const.DEFAULT_TWEEN_TIME).OnComplete(() => 
        {
            gameObject.SetActive(false);
            transform.localScale = Vector3.one;
        });
    }
}
