using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DarkcupGames;

public class ButtonSound : MonoBehaviour
{
    public AudioClip buttonSound;
    public float volume;
    bool addedSound = false;

    private void Awake()
    {
        volume = AudioSystem.VOLUME;
    }
    private void Start() {
        AddButtonSounds();
    }

    private void OnEnable() {
        AddButtonSounds();
    }

    public void AddButtonSounds() {
        if (addedSound) return;
        addedSound = true;

        Button[] buttons = gameObject.GetComponentsInChildren<Button>(true);
        for (int i = 0; i < buttons.Length; i++) {
            buttons[i].onClick.AddListener(PlayButtonSound);
        }
    }

    public void PlayButtonSound() {
        if(volume == 0) volume = AudioSystem.VOLUME;
        //AudioSystem.Instance.PlaySound(buttonSound);
        AudioSystem.Instance.PlaySound(buttonSound,volume);
    }
}
