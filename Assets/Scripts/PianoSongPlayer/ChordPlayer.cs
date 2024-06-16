using System.Collections.Generic;
using UnityEngine;
using DarkcupGames;

[System.Serializable]
public class Chord
{
    public string name;
    public List<int> keys;
}

public class ChordPlayer : MonoBehaviour, IPiano
{
    public List<Chord> chords;
    Chord currentChord;
    int currentNote;

    private void Start()
    {
        currentChord = chords.RandomElement();
        currentNote = currentChord.keys.RandomElement();
    }

    public List<int> GetNextChord()
    {
        Chord oldChord = currentChord;
        currentChord = chords.RandomElement();
        return oldChord.keys;
    }

    public int GetNextNote()
    {
        List<int> notes = new List<int>(currentChord.keys);
        notes.Remove(currentNote);
        int oldNote = currentNote;
        currentNote = notes.RandomElement();
        return oldNote;
    }
}