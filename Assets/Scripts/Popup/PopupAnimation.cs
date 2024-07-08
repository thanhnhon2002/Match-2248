using UnityEngine;
using DarkcupGames;
using TMPro;
using UnityEngine.UI;
using System.Collections;
using DG.Tweening;

public class PopupAnimation : Popup
{
    public const float START_SCALE = 0.8f;
    public const float FADE_TIME = 0.2f;

    [SerializeField] TextMeshProUGUI topic;
    [SerializeField] TextMeshProUGUI content;
    [SerializeField] AnimationPanel panel;
    [SerializeField] GameObject reward;
    [SerializeField] Button btnClaim;
    [SerializeField] SetTextPanel textPanel;
    [SerializeField] GameObject[] listAppear;
    SoundPopup soundPopup;
    WaitForSeconds wait015 = new WaitForSeconds(0.15f);
    WaitForSeconds wait01 = new WaitForSeconds(0.1f);
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
        var canvasGroup = GetComponent<CanvasGroup>();
        if (canvasGroup)
        {
            canvasGroup.alpha = 0;
            canvasGroup.DOFade(1f, FADE_TIME);
        }
        if (topic != null) EasyEffect.Appear(topic.gameObject, START_SCALE, 1, 0.15f);
        yield return wait015;
        if (content != null)
        {
            EasyEffect.Appear(content.gameObject, START_SCALE, 1, 0.15f);
            yield return wait015;
        }
        if (panel != null)
        {
            panel.gameObject.SetActive(true);
            yield return wait01;
        }
        if (reward != null)
        {
            EasyEffect.Appear(reward, START_SCALE, 1, 0.15f);
            yield return wait015;
        }
        if (listAppear != null && listAppear.Length > 0)
        {
            foreach (var child in listAppear)
            {
                EasyEffect.Appear(child, START_SCALE, 1, 0.2f);
                yield return wait015;
            }
        }
        if (btnClaim == null)
        {
            this.UnLockButton();
            yield break;
        }
        EasyEffect.Appear(btnClaim.gameObject, START_SCALE, 1, 0.15f, 1.2f, this.UnLockButton);
        btnClaim?.onClick.RemoveAllListeners();
        btnClaim?.onClick.AddListener(PopupManager.Instance.DeQueue);
        btnClaim?.onClick.AddListener(() => Disappear());
    }
    IEnumerator AnimationDisappear()
    {
        if (topic != null) EasyEffect.Disappear(topic.gameObject, 1, 0, 0.1f);
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
                yield return wait01;
            }
        }
        if (btnClaim != null) EasyEffect.Disappear(btnClaim.gameObject, 1, 0, 0.1f);
        yield return wait01;
        if (panel != null) panel.gameObject.SetActive(false);
    }
}
