using UnityEngine;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine.UI;
using System;


public class EffectPart:MonoBehaviour
{
    public List<Vector3> listScale = new List<Vector3>();
    public float speed;
    [SerializeField] Image background;
    public GameObject part;
    Sequence sequence;
    
    private void Awake()
    {
        sequence = DOTween.Sequence();
    }
    public void Animation(int i)
    {
        sequence.Append(background.transform.DOScale(listScale[i], speed).SetEase(Ease.Linear));
        i++;
        if (i == listScale.Count)
        {
            part.SetActive(true);
            return;
        }
        Animation(i);
    }
}