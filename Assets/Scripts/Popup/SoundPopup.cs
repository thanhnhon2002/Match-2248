using UnityEngine;
using DarkcupGames;
public class SoundPopup : MonoBehaviour
{
    public AudioClip popupSound;
    public void PlayPopupSound()
    {
        AudioSystem.Instance.PlaySound(popupSound);
    }
}
