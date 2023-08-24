using Coinbase.Exchange.SharedKernel.Models.Subscription;
using System.Globalization;

namespace Coinbase.Exchange.API.BackgroundServices
{
    public class OrderUpdateComparer : IComparer<decimal>
    {
        public int Compare(decimal x, decimal y)
        {
            return y.CompareTo(x);
        }
    }
}
