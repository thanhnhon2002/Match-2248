using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;

public class CellColor : MonoBehaviour
{
    public static CellColor Instance { get; private set; }
    [SerializeField] private Color[] colors;
    private void Awake ()
    {
        if(Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad (gameObject);
        }
        else Destroy (gameObject);
    }

    public Color GetCellColor (BigInteger value)
    {
        Mathf mathf;
        var pow = mathf.LogBigInt (value, 2);
        var index = pow % colors.Length;
        var color = colors[index];
        color.a = 1f;
        return color;
    }
}
