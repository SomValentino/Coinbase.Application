﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Coinbase.Exchange.SharedKernel.Models.Subscription
{
    public class Candle : SocketDataBase
    {
        public List<CandleEvent> Events { get; set; }
    }
}
