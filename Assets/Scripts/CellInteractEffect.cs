using DarkcupGames;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CellInteractEffect : MonoBehaviour
{
    private const float CELL_VOLUME = 0.2f;
    [SerializeField] private AudioClip[] clips;

    public void PlaySound()
    {
        var clip = clips.RandomElement ();
        AudioSystem.Instance.PlaySound(clip, CELL_VOLUME);
        var userData = GameSystem.userdata;
        if (userData.dicSetting[SettingKey.Vibration]) Handheld.Vibrate ();
    }
}
