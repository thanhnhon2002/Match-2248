using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DarkcupGames;
using System.IO;

public enum PianoPlayerType
{
    Song, Chord
}

public class PianoSongPlayer : MonoBehaviour
{
    public const string CHORD_IDENTIFIER = "###";
    public const float CHORD_KEY_DELAY_TIME = 0.05f;

    public static PianoSongPlayer Instance;
    public List<AudioClip> allPianoKeys;

    public PianoPlayerType type = PianoPlayerType.Chord;
    public TextSongPlayer textSongPlayer;
    public ChordPlayer chordPlayer;

    private Dictionary<string, AudioClip> dicPianoKey = new Dictionary<string, AudioClip>();
    private readonly WaitForSeconds wait = new WaitForSeconds(CHORD_KEY_DELAY_TIME);
    private readonly List<string> keyNames = new List<string>()
    {
        "C","D","E","F","G","A","B"
    };
    private string note;
    private void Awake()
    {
        Instance = this;
        Init();
    }

    public void Init()
    {
        dicPianoKey = new Dictionary<string, AudioClip>();
        for (int i = 0; i < allPianoKeys.Count; i++)
        {
            dicPianoKey.Add(allPianoKeys[i].name, allPianoKeys[i]);
        }
    }

    //[ContextMenu("Play Chord")]
    //private void Test()
    //{
    //    StartCoroutine(IEPlayChord(new List<int>() { 50, 52, 54}));
    //}

    private IEnumerator IEPlayChord(List<string> chord)
    {
        for (int i = 0; i < chord.Count; i++)
        {
            AudioSystem.Instance.PlaySound(dicPianoKey[chord[i]]);
            yield return wait;
        }
    }

    public void PlayNextNote()
    {
        if (type == PianoPlayerType.Song)
        {
            note = textSongPlayer.GetNextNote();
        } else
        {
            note = chordPlayer.GetNextNote();
        }
        AudioSystem.Instance.PlaySound(dicPianoKey[note]);
    }

    public void PlayNextChord()
    {
        List<string> chord;
        if(type == PianoPlayerType.Song)
        {
            chord = textSongPlayer.GetNextChord();
        } else
        {
            chord = chordPlayer.GetNextChord();
        }
        StartCoroutine(IEPlayChord(chord));
    }

#if UNITY_EDITOR
    [ContextMenu("Load and save all keys")]
    public void LoadAndSaveAllKeys()
    {
        for (int i = 3; i <= 7; i++)
        {
            for (int j = 0; j < keyNames.Count; j++)
            {
                string keyName = keyNames[j] + i;
                string filePath = @"D:\1. PERSONAL PROJECTS\MyProjects\Match-2248-\Assets\AudioClip\piano\piano-mp3\" + keyName + ".mp3";
                string newName = i.ToString() + j.ToString();
                string newPath = @"D:\1. PERSONAL PROJECTS\MyProjects\Match-2248-\Assets\AudioClip\piano\piano-mp3\Generated\" + newName + ".mp3";
                File.Copy(filePath, newPath, true);
            }
        }
    }
#endif
}