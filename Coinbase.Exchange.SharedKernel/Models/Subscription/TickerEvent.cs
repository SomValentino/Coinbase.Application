namespace Coinbase.Exchange.SharedKernel.Models.Subscription
{
    public class TickerEvent
    {
        public string Type { get; set; }
        public List<TickerDetails> Tickers { get; set; }
    }
}