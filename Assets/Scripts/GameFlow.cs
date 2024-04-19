using System;
using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;

public class GameFlow : MonoBehaviour
{
    public static GameFlow Instance { get; private set; }
    private const int INIT_MULTILIER = 30;
    public List<int> multiliers = new List<int>();
    private BigInteger totalPoint;
    public BigInteger TotalPoint
    {
        get { return totalPoint; }
        set { totalPoint = value; }
    }
    private void Awake ()
    {
        Instance = this;
        InitMultilier ();
    }

    private void InitMultilier ()
    {
        for (int i = 0; i <= INIT_MULTILIER; i++)
        {
            var pow = Mathf.Pow (2, i);
            multiliers.Add ((int)pow);
        }
    }

    public void CalculateTotal (BigInteger initValue, int cellCount)
    {
        TotalPoint = initValue * (BigInteger)Mathf.Pow(2, IndexCellCount(cellCount) + 1);
        Debug.Log(TotalPoint);
    }
    private int IndexCellCount (int cellCount)
    {
        if (cellCount == 0) return -1;
        for(var index=0;index<=INIT_MULTILIER;index++)
        {
            if (cellCount == multiliers[index]) return index;
            if (multiliers[index + 1] > cellCount && multiliers[index] <= cellCount)
                return index;
        }
        return multiliers.Count - 1;
    }
    //private void Start()
    //{
    //    Debug.Log(BigIntegerConverter.ConverNameValue(12));
    //    Debug.Log(BigIntegerConverter.ConverNameValue(13422));
    //    Debug.Log(BigIntegerConverter.ConverNameValue(3213422));
    //    Debug.Log(BigIntegerConverter.ConverNameValue(4213213422));
    //    Debug.Log(BigIntegerConverter.ConverNameValue(521321213422));
    //    Debug.Log(BigIntegerConverter.ConverNameValue(72234231321213422));
    //    Debug.Log(BigIntegerConverter.ConverNameValue(BigInteger.Parse("352522342325531235261661621213422")));
    //    Debug.Log(BigIntegerConverter.ConverNameValue(BigInteger.Parse("55151156352522342325531235261661621213422")));
    //    Debug.Log(BigIntegerConverter.ConverNameValue(BigInteger.Parse("18256651184851581529242312121185151156352522342325531235261661621213422")));
    //    Debug.Log(BigIntegerConverter.ConverNameValue(BigInteger.Parse("99999984651321651321561325415153205415231651532185123154658413215413285132452024653210351321651215121812316520234156320513209999999999999918256651184851581529242312121185151156352522342325531235261661621213422")));
    //}

}
