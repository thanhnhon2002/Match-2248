using UnityEngine;
using DarkcupGames;
using TMPro;
using UnityEngine.UI;
using System.Collections;

public class PopupAnimation : Popup
{
    [SerializeField] TextMeshProUGUI topic;
    [SerializeField] TextMeshProUGUI content;
    [SerializeField] AnimationPanel panel;
    [SerializeField] GameObject reward;
    [SerializeField] Button btnClaim;
    private void Awake()
    {
        panel.Awake();
        btnClaim.onClick.AddListener(() => Disappear());
    }
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
        if(panel != null)
        {
            panel.gameObject.SetActive(true);
            yield return new WaitForSeconds(0.15f);
        }      
        if(reward != null)
        {
            EasyEffect.Appear(reward, 0.5f, 1, 0.2f);
            yield return new WaitForSeconds(0.15f);
        }        
        EasyEffect.Appear(btnClaim.gameObject, 0.5f, 1, 0.2f);
    }
    IEnumerator AnimationDisappear()
    {
        EasyEffect.Disappear(topic.gameObject, 1, 0, 0.2f);
        EasyEffect.Disappear(content.gameObject, 1, 0, 0.2f);
        if (reward != null)
        {
            EasyEffect.Disappear(reward, 1, 0, 0.2f);
        }            
        EasyEffect.Disappear(btnClaim.gameObject, 1, 0, 0.2f);
        yield return new WaitForSeconds(0.15f);
        if (panel != null) panel.gameObject.SetActive(false);
    }
}
