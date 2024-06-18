using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class Rank
{
    public Dictionary<int, Dictionary<string, UserDataServer>> ranks = new Dictionary<int, Dictionary<string, UserDataServer>>();
}
