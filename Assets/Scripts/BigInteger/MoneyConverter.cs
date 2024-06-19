using System;

[Serializable]
public static class MoneyConveter
{
    public static string ConverNameValue(long value)
    {
        if (value < 1000) return value.ToString();
        if (value < 1000000) 
            if(value<10000) return (value / 1000).ToString() + ","+(value/100%10) + "K" ;
            else return (value / 1000).ToString() + "K";
        else if(value<1000000000) 
            if(value<10000000) return (value / 1000000).ToString() + "," + (value / 100000 % 10) + "M";
            else return (value / 1000000).ToString() + "M";
        else if (value < 10000000000) return (value / 1000000000).ToString() + "," + (value / 100000000 % 10) + "B";
            else return (value / 1000000000).ToString() + "B";
    }
}
