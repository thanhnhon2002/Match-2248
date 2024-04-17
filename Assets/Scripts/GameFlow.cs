using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameFlow : MonoBehaviour
{
    public static GameFlow Instance { get; private set; }
    private const int INIT_MULTILIER = 30;
    public List<int> multiliers = new List<int>();
    [SerializeField] private int totalPoint;
    public int TotalPoint
    {
        get { return totalPoint; }
        set { totalPoint = value; }
    }
    private void Awake ()
    {
        Instance = this;
        InitMultilier();
    }

    private void InitMultilier()
    {
        for (int i = 0; i <= INIT_MULTILIER; i++)
        {
            var pow = Mathf.Pow (2, i);
            multiliers.Add((int)pow);
        }
    }

    public void CalculateTotal (int initValue, int cellCount)
    {
        TotalPoint =(int)initValue * (int)Mathf.Pow(2,IndexCellCount(cellCount) + 1);
    }
    int IndexCellCount(int cellCount)
    {
        if (cellCount == 0) return -1;
        for(var index=0;index<=INIT_MULTILIER;index++)
        {
            if (cellCount == multiliers[index]) return index;
            if (multiliers[index + 1] > cellCount && multiliers[index]<=cellCount)
                return index;
        }
        return multiliers.Count-1;
    }
}
