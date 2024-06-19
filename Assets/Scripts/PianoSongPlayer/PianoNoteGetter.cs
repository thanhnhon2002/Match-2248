using System.Collections.Generic;
using UnityEngine;

public abstract class PianoNoteGetter : MonoBehaviour
{
    public abstract string GetNextNote();
    public abstract List<string> GetNextChord();
}