using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PianoNote
{
    public string key;
    public List<string> chord;
}

[System.Serializable]
public class PianoSong
{
    public List<PianoNote> notes = new List<PianoNote>();
}

public class TextSongPlayer : MonoBehaviour, IPiano
{
    public const string CHORD_IDENTIFIER = "###";
    public const float CHORD_KEY_DELAY_TIME = 0.05f;
    [SerializeField] private TextAsset textAsset;
    private PianoSong currentSong;
    private int current;
    private void Start()
    {
        currentSong = ReadSongFromText(textAsset.text);
    }

    public string GetNextNote()
    {
        current++;
        if (current >= currentSong.notes.Count) current = 0;
        var note = currentSong.notes[current];
        return note.key;
    }

    public List<string> GetNextChord()
    {
        current++;
        if (current >= currentSong.notes.Count) current = 0;
        var note = currentSong.notes[current];
        return note.chord;
    }

    private PianoSong ReadSongFromText(string text)
    {
        List<string> currentChord = new List<string>();
        PianoSong song = new PianoSong();
        song.notes = new List<PianoNote>();
        string[] lines = text.Split("\n");
        for (int i = 0; i < lines.Length; i++)
        {
            if (lines[i].StartsWith(CHORD_IDENTIFIER))
            {
                currentChord = ReadChord(lines[i]);
            }
            else
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
        note.key = key.ToString();
        return note;
    }

    List<string> ReadChord(string line)
    {
        line = line.Replace(CHORD_IDENTIFIER, "");
        List<string> result = new List<string>();
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
            result.Add(key.ToString());
        }
        return result;
    }
}
