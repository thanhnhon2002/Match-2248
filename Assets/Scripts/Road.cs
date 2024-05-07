using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Road : MonoBehaviour
{
    public RectTransform rectTransform;
    public CheckPoint[] checkPoints;
    private InfiniteScroll scroll;
    private void Awake ()
    {
        rectTransform = (RectTransform)transform;
        scroll = GetComponentInParent<InfiniteScroll>();
    }

    public IEnumerator UpdateDisplay()
    {
        if(scroll == null) scroll = GetComponentInParent<InfiniteScroll>();
        for (int i = 0; i < checkPoints.Length; i++)
        {
            var value = (BigInteger)Mathf.Pow(2, scroll.lastUpdateIndex);
            checkPoints[i].Display(value);
            scroll.lastUpdateIndex += 1;
            yield return new WaitForSeconds(0.1f);
        }
    }

    public IEnumerator UpdateDisplayRevres()
    {
        if (scroll == null) scroll = GetComponentInParent<InfiniteScroll>();
        for (int i = checkPoints.Length - 1; i >= 0; i--)
        {
            var value = (BigInteger)Mathf.Pow(2, scroll.lastUpdateIndex);
            checkPoints[i].Display(value);
            scroll.lastUpdateIndex -= 1;
            yield return new WaitForSeconds(0.1f);
        }
    }
}
