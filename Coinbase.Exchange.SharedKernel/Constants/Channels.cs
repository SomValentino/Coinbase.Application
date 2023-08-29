﻿namespace Coinbase.Exchange.SharedKernel.Constants
{
    public static class Channels
    {
        public static string[] InputChannels = new[] { "ticker", "level2", "candles" };
        public static string[] OutputChannels = new[] { "ticker", "l2_data", "candles" };
    }
}
