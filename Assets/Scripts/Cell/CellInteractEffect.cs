using DarkcupGames;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CellInteractEffect : MonoBehaviour
{
    private const long VIBRATION_STRENGTH = 20;
    public const float VOLUME = 0.5f;

    [SerializeField] private AudioClip[] clips;

    public void PlaySound()
    {
        //Debug.LogError("play sound!");
        PianoSongPlayer.Instance.PlayNextNote(VOLUME);
        Vibration.Vibrate(VIBRATION_STRENGTH);
    }
}