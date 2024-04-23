using UnityEngine;
using DarkcupGames;
using TMPro;
using UnityEngine.UI;
using System.Collections;

public class PopupBlockAdded : Popup
{
    [SerializeField] TextMeshProUGUI topic;
    [SerializeField] TextMeshProUGUI content;
    [SerializeField] AnimationPanelBlock panel;
    [SerializeField] Button btnOK;

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
        EasyEffect.Appear(content.gameObject, 0.2f, 1, 0.2f);
        yield return new WaitForSeconds(0.2f);
        panel.gameObject.SetActive(true);
        yield return new WaitForSeconds(0.15f);
        EasyEffect.Appear(btnOK.gameObject, 0.5f, 1, 0.2f);
    }
    IEnumerator AnimationDisappear()
    {
        EasyEffect.Disappear(topic.gameObject, 1, 0, 0.2f);
        EasyEffect.Disappear(content.gameObject, 1, 0, 0.2f);
        EasyEffect.Disappear(btnOK.gameObject, 1, 0, 0.2f);
        yield return new WaitForSeconds(0.15f);
        panel.gameObject.SetActive(false);
    }
}
