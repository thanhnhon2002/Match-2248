using System.Collections.Generic;

public interface IPiano
{
    public string GetNextNote();
    public List<string> GetNextChord();
}