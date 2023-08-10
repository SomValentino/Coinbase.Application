using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Coinbase.Exchange.SharedKernel.Models.Order
{
    public class OrderConfiguration
    {
        public MarketIoc Market_market_ioc { get; set; }
        public LimitGtc Limit_limit_gtc { get; set; }
        public LimitGtd Limit_limit_gtd { get; set; }
        public StopLimitGtc Stop_limit_stop_limit_gtc { get; set; }
        public StopLimitGtd Stop_limit_stop_limit_gtd { get; set; }
    }
}
