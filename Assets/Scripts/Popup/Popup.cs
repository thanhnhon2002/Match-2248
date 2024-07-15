using UnityEngine;
using DarkcupGames;
using DG.Tweening;
using UnityEngine.UI;
using System.Collections;
using Unity.VisualScripting;

public class Popup : MonoBehaviour
{
    public PopupOptions option;
    public Vector3 originSize;
    public Button[] buttons;
    protected virtual void Awake()
    {
        originSize = transform.localScale;
        buttons = GetComponentsInChildren<Button>(true);
        foreach (Button button in buttons)
        {
            button.onClick.AddListener(() => StartUnLock());
        }
    }
    public virtual void Appear()
    {
        this.Awake();
        LockButton();
        transform.localScale = originSize;
        GetComponent<CanvasGroup>().alpha = 0;
        if (PopupManager.Instance.blackBackground != null) PopupManager.Instance.blackBackground.gameObject.SetActive(true);
        GetComponent<CanvasGroup>().DOFade(1,0.3f);
        EasyEffect.Appear(gameObject, 0.5f, 1, 0.2f);
    }
    public virtual void Disappear()
    {
        transform.localScale = originSize;
        GetComponent<CanvasGroup>().alpha = 1;
        GetComponent<CanvasGroup>().DOFade(0, 0.3f);
        EasyEffect.Disappear(gameObject, 1, 0, 0.2f);
    }
    public void LockButton()
    {
        foreach (Button button in buttons)
        {
            button.interactable = false;
        }
    }
    public void UnLockButton()
    {
        foreach (Button button in buttons)
        {
            button.interactable = true;
        }
    }
    public void StartUnLock()
    {
        StartCoroutine(TimeUnLockButton());
    }
    IEnumerator TimeUnLockButton()
    {
        if (buttons[0].interactable == false) yield break;
        LockButton();
        yield return new WaitForSeconds(0.2f);
        UnLockButton();
    }
}
