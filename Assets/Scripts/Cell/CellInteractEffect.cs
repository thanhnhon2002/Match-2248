using DarkcupGames;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CellInteractEffect : MonoBehaviour
{
    private const long VIBRATION_STRENGTH = 20;
    [SerializeField] private AudioClip[] clips;

    public void PlaySound()
    {
        PianoSongPlayer.Instance.PlayNextNote();
        Vibration.Vibrate(VIBRATION_STRENGTH);
    }
}