using System;

[Serializable]
public static class MoneyConveter
{
    public static string ConvertNameValue(long value)
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
    public static string ConvertNameValueBestCode(long value)
    {
        if (value < 1000) return value.ToString();
        for(int i=3;i>0;i--)
        {
            if (value / (long)Math.Pow(10, i*3) > 0) return Convert(i,value);
        }
        return "";
    }
    static string Convert(int i, long value)
    {
        string extension = new NameExtension(i).name;
        if (value / (long)Math.Pow(10, i * 3 + 1) == 0) return (value / (long)Math.Pow(10, i * 3)).ToString() + "," + (value / (long)Math.Pow(10, i * 3-1) % 10) + extension;
        else return (value / (long)Math.Pow(10, i * 3)).ToString() + extension;
    }
}
