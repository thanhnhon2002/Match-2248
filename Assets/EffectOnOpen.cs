using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectOnOpen : MonoBehaviour
{
    [SerializeField] private CanvasGroup parent;
    [SerializeField] private CanvasGroup[] fadeElenemt;
    [SerializeField] private float fadeTime;
    [SerializeField] private float delayTime;

    private void OnEnable ()
    {
        transform.localScale = Vector3.zero;
        parent.alpha = 0f;
        for (int i = 0; i < fadeElenemt.Length; i++)
        {
            fadeElenemt[i].alpha = 0f;
        }
        StartCoroutine (DoEffect ());
    }

    private IEnumerator DoEffect()
    {
        transform.DOScale (1f, fadeTime);
        parent.DOFade (1f, fadeTime);
        yield return new WaitForSeconds (fadeTime);
        for (int i = 0; i < fadeElenemt.Length; i++)
        {
            fadeElenemt[i].DOFade (1f, fadeTime);
            yield return new WaitForSeconds (delayTime);
        }
    }
}
