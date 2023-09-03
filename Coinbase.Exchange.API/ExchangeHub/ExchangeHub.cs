
using Coinbase.Exchange.Domain.Entities;
using Coinbase.Exchange.Domain.Specifications;
using Coinbase.Exchange.Infrastructure.Repository;
using Coinbase.Exchange.Logic.DataFeed;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;

namespace Coinbase.Exchange.API.ExchangeHub
{
    [Authorize]
    public class ExchangeHub: Hub
    {
        private readonly IRepository<Domain.Entities.Client> _clientRepository;
        private readonly IRepository<Instrument> _instrumentRepository;
        private readonly ICoinbaseWebSocketClient _webSocketClient;
        private readonly ILogger<ExchangeHub> _logger;

        public ExchangeHub(ICoinbaseWebSocketClient coinbaseWebSocketClient,
            IRepository<Domain.Entities.Client> clientRepository,
            IRepository<Instrument> instrumentRepository,
            ILogger<ExchangeHub> logger)
        {
            _clientRepository = clientRepository;
            _instrumentRepository = instrumentRepository;
            _webSocketClient = coinbaseWebSocketClient;
            _logger = logger;
        }

        private string ClientId => Context.User!.Identity!.Name;
        private string? ClientName => Context.User!.Claims.FirstOrDefault(_ => _.Type == "name")?.Value;

        public override async Task OnConnectedAsync()
        {

            try
            {
                var client = await _clientRepository.GetBySpecAsync(new InstrumentsByClientIdSpec(ClientId));

                if (client == null)
                    throw new ArgumentException($"Client with Id: {ClientId}");
                foreach (var group in client.ProductGroups)
                {
                    await _webSocketClient.SubScribe(new[] { group.Name });
                    await Groups.AddToGroupAsync(Context.ConnectionId, group.Name);
                }
                await base.OnConnectedAsync();
            }
            catch (Exception ex)
            {

                _logger.LogError(ex.ToString());
            }
        }

        public async Task SubscribeAsync(string instrument)
        {

            try
            {
                var group = await _instrumentRepository.GetBySpecAsync(new ClientsByProductGroupNameSpec(instrument));

                var client = await _clientRepository.GetByIdAsync(ClientId);

                if (group == null)
                {
                    group = await _instrumentRepository.AddAsync(new Instrument
                    {
                        Name = instrument
                    });
                    await _webSocketClient.SubScribe(new[] { instrument });
                }

                var isMember = group.Clients.Any(_ => _.ClientId == ClientId);

                if (!isMember)
                {
                    if (client != null)
                    {
                        group.Clients.Add(client);
                    }
                    else
                    {
                        group.Clients.Add(new Domain.Entities.Client
                        {
                            ClientId = ClientId,
                            ClientName = ClientName!
                        });
                    }
                }


                await _instrumentRepository.SaveChangesAsync();

                await Groups.AddToGroupAsync(Context.ConnectionId, instrument);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString());
            }
        }

        public async Task UnSubscribeAsync(string instrument)
        {
            try
            {
                var group = await _instrumentRepository.GetBySpecAsync(new ClientsByProductGroupNameSpec(instrument));

                if (group == null)
                {
                    throw new ArgumentNullException($"Client with Id: {ClientId} cannot unsubscribe from group that does not exist");
                }

                var client = await _clientRepository.GetByIdAsync(ClientId);

                if (client == null)
                {
                    throw new ArgumentNullException($"Client with Id: {ClientId} does not exist");
                }

                group.Clients.Remove(client);

                if (!group.Clients.Any())
                {
                    await _webSocketClient.UnSubscribe(new[] { group.Name });
                }

                await _instrumentRepository.SaveChangesAsync();
                await Groups.RemoveFromGroupAsync(Context.ConnectionId, instrument);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString());
            }
        }

        public async Task SubscribeMultipleAsync(IEnumerable<string> instruments)
        {
            foreach (var instrument in instruments)
            {
                await SubscribeAsync(instrument);
            }
        }

        public async Task UnSubscribeMultipleAsync(IEnumerable<string> instruments)
        {
            foreach(var instrument in instruments)
            {
                await UnSubscribeAsync(instrument);
            }
        }
    }
}
