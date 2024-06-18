using System.Collections.Generic;
using UnityEngine;
using DarkcupGames;

[System.Serializable]
public class Chord
{
    public string name;
    public List<string> keys;
}

public class ChordPlayer : MonoBehaviour, IPiano
{
    public List<Chord> chords;
    Chord currentChord;
    string currentNote;

    private void Start()
    {
        currentChord = chords.RandomElement();
        currentNote = currentChord.keys.RandomElement();
    }

    public List<string> GetNextChord()
    {
        Chord oldChord = currentChord;
        currentChord = chords.RandomElement();
        return oldChord.keys;
    }

    public string GetNextNote()
    {
        List<string> notes = new List<string>(currentChord.keys);
        notes.Remove(currentNote);
        string oldNote = currentNote;
        currentNote = notes.RandomElement();
        return oldNote;
    }

    [ContextMenu("Test play chord")]
    public void TestChord()
    {
        PianoSongPlayer.Instance.PlayNextChord();
    }
}