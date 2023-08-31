namespace Coinbase.Exchange.SharedKernel.Constants
{
    public static class Channels
    {
        public const string Ticker = "ticker";
        public const string Level2 = "level2";
        public const string Candles = "candles";
        public const string L2_Data = "l2_data";
        public static string[] InputChannels = new[] { Ticker, Level2, Candles };
        public static string[] OutputChannels = new[] { Ticker, L2_Data, Candles};
    }
}
