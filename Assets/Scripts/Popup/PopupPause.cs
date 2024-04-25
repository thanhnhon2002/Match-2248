using UnityEngine;
using DarkcupGames;
using TMPro;
using UnityEngine.UI;
using System.Collections;

public class PopupPause : Popup
{
    [SerializeField] TextMeshProUGUI topic;

    public override void Appear()
    {
        base.Appear();
        StartCoroutine(AnimationAppear());
    }
    public override void Disappear()
    {
        StartCoroutine(AnimationDisappear());
        base.Disappear();
    }
    IEnumerator AnimationAppear()
    {
        EasyEffect.Appear(topic.gameObject, 0.2f, 1, 0.2f);
        yield return new WaitForSeconds(0.15f);
        foreach(var btn in buttons)
        {
            EasyEffect.Appear(btn.gameObject, 0.2f, 1, 0.2f);
            yield return new WaitForSeconds(0.15f);
        }
        yield return new WaitForSeconds(0.15f);
        UnLockButton();
    }
    IEnumerator AnimationDisappear()
    {
        EasyEffect.Disappear(topic.gameObject, 1, 0, 0.2f);
        yield return new WaitForEndOfFrame();
        foreach (var btn in buttons)
        {
            EasyEffect.Disappear(btn.gameObject, 1, 0, 0.2f);
        }
    }
}
