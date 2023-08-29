namespace Coinbase.Exchange.SharedKernel.Models.Subscription
{
    public class CandleDetails
    {
        public decimal Start { get; set; }
        public decimal High { get; set; }
        public decimal Low { get; set; }
        public decimal Open { get; set; }
        public decimal Close { get; set; }
        public decimal Volume { get; set; }
        public DateTime Date { get; set; }
        public string product_id { get; set; }
    }
}