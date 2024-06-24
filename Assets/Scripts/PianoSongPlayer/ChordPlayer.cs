using System.Collections.Generic;
using UnityEngine;
using DarkcupGames;

[System.Serializable]
public class Chord
{
    public string name;
    public List<string> keys;
    public List<string> chords;
}

public class ChordPlayer : PianoNoteGetter
{
    public List<Chord> chords;
    Chord currentChord;
    string currentNote;

    private void Start()
    {
        currentChord = chords.RandomElement();
        currentNote = currentChord.keys.RandomElement();
    }

    public override List<string> GetNextChord()
    {
        Chord oldChord = currentChord;
        currentChord = chords.RandomElement();
        return new List<string>() { oldChord.chords.RandomElement() };
    }

    public override string GetNextNote()
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