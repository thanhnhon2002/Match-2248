using com.adjust.sdk;
using GoogleMobileAds.Api;

public class AdjustLog
{
    public static void OnAdRevenuePaidEventMAX(MaxSdkBase.AdInfo info)
    {
        var adRevenue = new AdjustAdRevenue(AdjustConfig.AdjustAdRevenueSourceAppLovinMAX);
        adRevenue.setRevenue(info.Revenue, "USD");
        adRevenue.setAdRevenueNetwork(info.NetworkName);
        adRevenue.setAdRevenueUnit(info.AdUnitIdentifier);
        adRevenue.setAdRevenuePlacement(info.Placement);

        Adjust.trackAdRevenue(adRevenue);
    }

    public static void OnAdRevenuePaidEventAdmob(AdValue args)
    {
        AdjustAdRevenue adRevenue = new AdjustAdRevenue(AdjustConfig.AdjustAdRevenueSourceAdMob);
        adRevenue.setRevenue(args.Value / 1000000f, args.CurrencyCode);
        Adjust.trackAdRevenue(adRevenue);
    }
}
