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
    [SerializeField] SetTextPanel textPanel;
    [SerializeField] GameObject[] listAppear;
    SoundPopup soundPopup;
    public override void Appear()
    {
        if (gameObject.activeInHierarchy) return;
        base.Appear();
        if (transform.TryGetComponent<SoundPopup>(out soundPopup)) soundPopup.PlayPopupSound();
        if (textPanel != null) textPanel.SetText();
        StartCoroutine(AnimationAppear());
    }
    public override void Disappear()
    {
        StartCoroutine(AnimationDisappear());
        base.Disappear();
    }
    IEnumerator AnimationAppear()
    {
        if (topic != null) EasyEffect.Appear(topic.gameObject, 0.2f, 1, 0.15f);
        yield return new WaitForSeconds(0.15f);
        if (content != null)
        {
            EasyEffect.Appear(content.gameObject, 0.2f, 1, 0.15f);
            yield return new WaitForSeconds(0.15f);
        }
        if (panel != null)
        {
            panel.gameObject.SetActive(true);
            yield return new WaitForSeconds(0.1f);
        }
        if (reward != null)
        {
            EasyEffect.Appear(reward, 0.5f, 1, 0.15f);
            yield return new WaitForSeconds(0.15f);
        }
        if (listAppear != null && listAppear.Length > 0)
        {
            foreach (var child in listAppear)
            {
                EasyEffect.Appear(child, 0.5f, 1, 0.2f);
                yield return new WaitForSeconds(0.15f);
            }
        }
        if (btnClaim == null)
        {
            this.UnLockButton();
            yield break;
        }
        EasyEffect.Appear(btnClaim.gameObject, 0.5f, 1, 0.15f, 1.2f, this.UnLockButton);
        btnClaim?.onClick.RemoveAllListeners();
        btnClaim?.onClick.AddListener(PopupManager.Instance.DeQueue);
        btnClaim?.onClick.AddListener(() => Disappear());
    }
    IEnumerator AnimationDisappear()
    {
        EasyEffect.Disappear(topic.gameObject, 1, 0, 0.1f);
        if (content != null) EasyEffect.Disappear(content.gameObject, 1, 0, 0.1f);
        if (reward != null)
        {
            EasyEffect.Disappear(reward, 1, 0, 0.1f);
        }
        if (listAppear != null && listAppear.Length > 0)
        {
            foreach (var child in listAppear)
            {
                EasyEffect.Disappear(child, 1, 0, 0.1f);
                yield return new WaitForSeconds(0.1f);
            }
        }
        if (btnClaim != null) EasyEffect.Disappear(btnClaim.gameObject, 1, 0, 0.1f);
        yield return new WaitForSeconds(0.1f);
        if (panel != null) panel.gameObject.SetActive(false);
    }
}
