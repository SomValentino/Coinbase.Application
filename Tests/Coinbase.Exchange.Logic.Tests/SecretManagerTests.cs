using Coinbase.Exchange.Logic.Security;
using FluentAssertions;
using Moq;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Coinbase.Exchange.Logic.Tests
{
    public class SecretManagerTests
    {
        private readonly ISecretManager _secretManager;

        public SecretManagerTests()
        {
            _secretManager = new SecretManager();
        }

        [Fact]
        public void SecretManager_WhenGivenBody_GeneratesSignature()
        {
            var secret = "test";

            var timestamp = 1691494973;
            Debug.WriteLine(timestamp);
            var url = new Uri("https://coinbase.com/api/v3/brokerage/products/BTC-USD/ticker?limit=3");
            var request_path = "/api/v3/brokerage/orders/historical/batch";
            var body = "";
            var method = "GET";

            var payload = $"{timestamp}{method}{request_path}{body}";

            var signature = _secretManager.GetSignature(payload, secret);
            Debug.WriteLine(signature);

            signature.Should().NotBeNull();
            signature.Should().NotBeEmpty();
            signature.Should().Be("400def43d139b6fd8176621ae78f873e16579b025535ecc0c280282a624f2644");
        }
    }
}
