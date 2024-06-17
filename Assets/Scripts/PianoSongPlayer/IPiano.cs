using System.Collections.Generic;

public interface IPiano
{
    public int GetNextNote();
    public List<int> GetNextChord();
}