using UnityEngine;
using DarkcupGames;
using DG.Tweening;

public class Popup : MonoBehaviour
{
    public PopupOptions option;
    public virtual void Appear()
    {
        EasyEffect.Fade(PopupManager.Instance.blackBackground.gameObject, 0.3f, 0.9f, true,0.2f);
        EasyEffect.Appear(gameObject, 0.5f, 1,0.2f);
    }
    public virtual void Disappear()
    {
        EasyEffect.Fade(PopupManager.Instance.blackBackground.gameObject, 0.9f, 0f, false,0.2f);
        EasyEffect.Disappear(gameObject, 1 , 0,0.2f);
    }
}
