using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DarkcupGames
{
    [RequireComponent (typeof (AudioSource))]
    public class AudioSystem : MonoBehaviour
    {
        public static AudioSystem Instance;

        public const int CHANEL_AMOUNT = 20;
        public const float VOLUME = 1f;

        [SerializeField] private List<AudioClip> fxSounds = new List<AudioClip> ();

        private Dictionary<string, AudioClip> clips = new Dictionary<string, AudioClip> ();
        [SerializeField] private AudioSource[] chanels;

        void Awake ()
        {
            if (Instance == null)
            {
                Instance = this;
                gameObject.transform.SetParent (null);
                DontDestroyOnLoad (gameObject);

            } else
            {
                Destroy (gameObject);
                return;
            }
            chanels = new AudioSource[CHANEL_AMOUNT];
            for (int i = 0; i < CHANEL_AMOUNT; i++)
            {
                chanels[i] = new GameObject ().AddComponent<AudioSource> ();
                chanels[i].transform.SetParent (transform);
            }
            for (int i = 0; i < fxSounds.Count; i++)
            {
                clips.Add (fxSounds[i].name, fxSounds[i]);
            }
        }

        public void PlaySound (AudioClip clip, float volume = VOLUME)
        {
            if (GameSystem.userdata.dicSetting[SettingKey.Sound] == false) return;

            foreach (var item in chanels)
            {
                if (!item.isPlaying)
                {
                    item.clip = clip;
                    item.volume = volume;
                    item.Play ();
                    break;
                }
            }
        }

        public void PlaySound(string soundName, float volume = VOLUME)
        {
            if (GameSystem.userdata.dicSetting[SettingKey.Sound] == false) return;
            var clip = clips[soundName];
            foreach (var item in chanels)
            {
                if (!item.isPlaying)
                {
                    item.clip = clip;
                    item.volume = volume;
                    item.Play ();
                    break;
                }
            }
        }
    }
}