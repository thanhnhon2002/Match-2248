using System.Collections.Generic;
using UnityEngine;
using DarkcupGames;

public class ChordPlayerOriginal : PianoNoteGetter
{
    public List<AudioClip> keys;
    public List<AudioClip> chords;

    public override List<string> GetNextChord()
    {
        return new List<string> { chords.RandomElement().name };
    }

    public override string GetNextNote()
    {
        return keys.RandomElement().name;
    }
}