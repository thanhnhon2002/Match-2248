namespace DeepTrackSDK
{
    [System.Serializable]
    public class DeepTrackUserRevenue
    {
        public int interSuccess;
        public int interFailed;
        public double interRevenue;

        public int rewardSuccess;
        public int rewardFailed;
        public double rewardRevenue;

        public int appOpenSuccess;
        public int appOpenFailed;
        public double appOpenRevenue;

        public int bannerSuccess;
        public int bannerFailed;
        public double bannerRevenue;
    }
}