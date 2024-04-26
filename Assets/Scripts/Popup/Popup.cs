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
            button.onClick.AddListener(()=>StartUnLock());
        }
    }
    public virtual void Appear()
    {   
        this.Awake();
        LockButton();
        transform.localScale = originSize;
        EasyEffect.Fade(PopupManager.Instance.blackBackground.gameObject, 0.3f, 0.9f, true,0.2f);
        EasyEffect.Appear(gameObject, 0.5f, 1,0.2f);
    }
    public virtual void Disappear()
    {
        transform.localScale = originSize;
        EasyEffect.Fade(PopupManager.Instance.blackBackground.gameObject, 0.9f, 0f, false,0.2f);
        EasyEffect.Disappear(gameObject, 1 , 0,0.2f);
    }
    public void LockButton()
    {
        foreach(Button button in buttons)
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
