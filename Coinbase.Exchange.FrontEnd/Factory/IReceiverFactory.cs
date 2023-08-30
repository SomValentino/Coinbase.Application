using Coinbase.Exchange.FrontEnd.Receivers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Coinbase.Exchange.FrontEnd.Factory
{
    public interface IReceiverFactory
    {
        DataReceiver GetDataReceiver(string type);
    }
}
