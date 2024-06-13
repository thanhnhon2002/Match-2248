using System;
using System.Numerics;
using Unity.Mathematics;
using UnityEngine;

[Serializable]
public static class BigIntegerConverter
{
    public static string ConverNameValue (BigInteger value)
    {
        if (value < 10000) return value.ToString ();
        string nameResult = "";
        for (int i = 1; i < int.MaxValue; i++)
        {
            if (value / (BigInteger)Math.Pow (10, i * 3) > 0 && value / (BigInteger)Math.Pow (10, (i + 1) * 3) == 0)
            {
                nameResult = (value / (BigInteger)Math.Pow (10, i * 3)).ToString ();
                NameExtension nameExtension = new NameExtension (i);
                nameResult += nameExtension.name;
                break;
            }
        }
        return nameResult;
    }
    public static int LogBigInt (this Mathf mathf, BigInteger num, int index)
    {
        if (num == 1) return 0;
        BigInteger bigInteger = 1;
        for (int i = 1; i < int.MaxValue; i++)
        {
            bigInteger *= index;
            if (bigInteger == num) return i;
            else if (bigInteger > num)
            {
                break;
            }
        }
        return -1;
    }

}
public class NameExtension
{
    public string name;
    public NameExtension (int index)
    {
        switch (index)
        {
            case 1:
                name = "K";
                break;
            case 2:
                name = "M";
                break;
            case 3:
                name = "B";
                break;
            default:
                name = Convert (index);
                break;
        }
    }
    string Convert (int index)
    {
        //string nameResult = "";
        int indexPow = index - 4;
        //int thuong = indexPow / 26;
        if (indexPow > 25)
        {
            //nameResult += ExtensionByIndex(thuong - 1);
            //int du = indexPow % 26;
            //nameResult += ExtensionByIndex(du);
            //return nameResult;
            return GetLimitExtension(indexPow);
        } else return ExtensionByIndex (indexPow);
    }
    string GetLimitExtension(int indexPow)
    {
        if(indexPow<26) return ExtensionByIndex(indexPow-1);
        else return GetLimitExtension(indexPow/26) + ExtensionByIndex(indexPow%26);
    }
    string ExtensionByIndex (int index)
    {
        for (char c = 'a'; c <= 'z'; c++)
        {
            if (index == c - 'a') return c.ToString ();
        }
        return "";
    }
}
