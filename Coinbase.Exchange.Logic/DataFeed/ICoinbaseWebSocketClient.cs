using Coinbase.Exchange.SharedKernel.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Coinbase.Exchange.Logic.DataFeed
{
    public interface ICoinbaseWebSocketClient
    {
        Task SubScribe(Uri uri,IEnumerable<string> instruments, WebSocketChannel channel);
        Task UnSubscribe(Uri uri, IEnumerable<string> instruments, WebSocketChannel channel);
    }
}
