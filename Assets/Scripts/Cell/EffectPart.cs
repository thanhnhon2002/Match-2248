using UnityEngine;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine.UI;
using System;


public class EffectPart : MonoBehaviour
{
    Sequence sequence;
    public List<Vector3> listScale = new List<Vector3>();
    public float speed=0.1f;
    public GameObject part;
    public GameObject topic;

    private void Awake()
    {
        sequence = DOTween.Sequence();
        topic.GetComponent<CanvasGroup>().alpha = 0;
    }
    public void Animation(int i)
    {
        if (i == 0)
        {
            sequence = DOTween.Sequence();
            if (Tutorial.instance.currentPart > 0)
            {
                sequence.AppendInterval(1f);
                sequence.AppendCallback(()=>Tutorial.instance.effects[Tutorial.instance.currentPart-1].part.gameObject.SetActive(false));
            }
            sequence.AppendCallback(()=> part.SetActive(true));
            sequence.Append(Tutorial.instance.background.transform.DOScale(listScale[i], 0).SetEase(Ease.Linear));
        }
        else sequence.Append(Tutorial.instance.background.transform.DOScale(listScale[i], speed).SetEase(Ease.InBack));
        if (i == 0)
        {          
            sequence.AppendCallback(() =>
            {               
                topic.GetComponent<CanvasGroup>().DOFade(1, 1f);
            });
            sequence.AppendInterval(0.2f);
        }
        i++;
        if (i == listScale.Count)
        {           
            return;
        }
        Animation(i);
    }
}