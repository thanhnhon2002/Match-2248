using DarkcupGames;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CellSound : MonoBehaviour
{
    [SerializeField] private AudioClip[] clips;

    public void PlaySound()
    {
        var clip = clips.RandomElement ();
        AudioSystem.Instance.PlaySound(clip);
    }
}
