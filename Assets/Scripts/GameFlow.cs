using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameFlow : MonoBehaviour
{
    public static GameFlow Instance { get; private set; }
    private const int INIT_MULTILIER = 6;
    private List<int> multiliers = new List<int>();
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
        for (int i = 1; i < INIT_MULTILIER; i++)
        {
            var pow = Mathf.Pow (2, i);
            for (int j = 0; j < pow; j++)
            {
                multiliers.Add ((int)pow);
            }
        }
    }

    public void CalculateTotal (int initValue, int cellCount)
    {
        var multilier = multiliers[cellCount - 2];
        TotalPoint = initValue * multilier;
    }
}
