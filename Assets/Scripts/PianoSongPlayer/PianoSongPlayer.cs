using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DarkcupGames;
using System.IO;

[System.Serializable]
public class PianoNote
{
    public int key;
    public List<int> chord;
}

[System.Serializable]
public class PianoSong
{
    public List<PianoNote> notes = new List<PianoNote>();
}

public class PianoSongPlayer : MonoBehaviour
{
    public const string CHORD_IDENTIFIER = "###";
    public const float CHORD_KEY_DELAY_TIME = 0.05f;

    public static PianoSongPlayer Instance;
    public List<AudioClip> allPianoKeys;
    public readonly List<string> keyNames = new List<string>()
    {
        "C","D","E","F","G","A","B"
    };
    public PianoSong currentSong;
    public TextAsset textAsset;
    private Dictionary<int, AudioClip> dicPianoKey = new Dictionary<int, AudioClip>();
    private int current = -1;
    private readonly WaitForSeconds wait = new WaitForSeconds(CHORD_KEY_DELAY_TIME);
    private void Awake()
    {
        Instance = this;
        Init();
    }

    private void Start()
    {
        currentSong = ReadSongFromText(textAsset.text);
    }
    public void Init()
    {
        dicPianoKey = new Dictionary<int, AudioClip>();
        for (int i = 0; i < allPianoKeys.Count; i++)
        {
            bool success = int.TryParse(allPianoKeys[i].name, out int key);
            if (!success)
            {
                Debug.LogError($"piano key file should name as integer, failed at name = {allPianoKeys[i].name}");
                continue;
            }
            dicPianoKey.Add(key, allPianoKeys[i]);
        }
    }

    [ContextMenu("Play Chord")]
    private void Test()
    {
        StartCoroutine(IEPlayerChord(new List<int>() { 50, 52, 54}));
    }

    private IEnumerator IEPlayerChord(List<int> chord)
    {
        for (int i = 0; i < chord.Count; i++)
        {
            AudioSystem.Instance.PlaySound(dicPianoKey[chord[i]]);
            yield return wait;
        }
    }

    private PianoSong ReadSongFromText(string text)
    {
        List<int> currentChord = new List<int>();
        PianoSong song = new PianoSong();
        song.notes = new List<PianoNote>();
        string[] lines = text.Split("\n");
        for (int i = 0; i < lines.Length; i++)
        {
            if (lines[i].StartsWith(CHORD_IDENTIFIER))
            {
                currentChord = ReadChord(lines[i]);
            } else
            {
                var note = ReadNote(lines[i]);
                if (note == null) continue;
                note.chord = currentChord;
                song.notes.Add(note);
            }
        }
        return song;
    }

    PianoNote ReadNote(string line)
    {
        if (line.Trim() == "") return null;
        int.TryParse(line, out int key);
        if (key == 0)
        {
            Debug.LogError("can not parse note from line = " + line);
            return null;
        }
        var note = new PianoNote();
        note.key = key;
        return note;
    }

    List<int> ReadChord(string line)
    {
        line = line.Replace(CHORD_IDENTIFIER, "");
        List<int> result = new List<int>();
        int first = line.IndexOf("//");
        if (first >= 0)
        {
            line = line.Substring(0, first).Trim();
        }
        var keys = line.Split(" ");
        bool success;
        for (int i = 0; i < keys.Length; i++)
        {
            if (keys[i] == "" || keys[i] == " ") continue;
            success = int.TryParse(keys[i], out int key);
            if (!success)
            {
                Debug.LogError("failed to read key = " + keys[i]);
                continue;
            }
            result.Add(key);
        }
        return result;
    }

    public void PlayNextNote()
    {
        current++;
        if (current >= currentSong.notes.Count) current = 0;
        var note = currentSong.notes[current];
        AudioSystem.Instance.PlaySound(dicPianoKey[note.key]);
    }

    public void PlayNextChord()
    {
        current++;
        if (current >= currentSong.notes.Count) current = 0;
        var note = currentSong.notes[current];
        AudioSystem.Instance.PlaySound(dicPianoKey[note.key]);
        StartCoroutine(IEPlayerChord(note.chord));
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