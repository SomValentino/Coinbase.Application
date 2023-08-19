﻿
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

        public ExchangeHub(ICoinbaseWebSocketClient coinbaseWebSocketClient,
            IRepository<Domain.Entities.Client> clientRepository,
            IRepository<Instrument> instrumentRepository)
        {
            _clientRepository = clientRepository;
            _instrumentRepository = instrumentRepository;
            _webSocketClient = coinbaseWebSocketClient;
        }

        private string ClientId => Context.User!.Identity!.Name;

        public override async Task OnConnectedAsync()
        {
            
            var client = await _clientRepository.GetByIdAsync(ClientId);

            if (client == null)
                throw new ArgumentException($"Client with Id: {ClientId}");
            foreach(var group in client.ProductGroups)
            {
                await Groups.AddToGroupAsync(Context.ConnectionId, group.Name);
            }
            await base.OnConnectedAsync();
        }

        public async Task SubscribeAsync(string instrument)
        {
            
            var group = await _instrumentRepository.GetBySpecAsync(new ClientsByProductGroupNameSpec(instrument));

            if(group == null)
            {
                group = await _instrumentRepository.AddAsync(new Instrument
                {
                    Name = instrument
                });

                group.Clients.Add(new Domain.Entities.Client
                {
                    ClientId = ClientId
                });
                await _webSocketClient.SubScribe(new[] { instrument });
                await _instrumentRepository.SaveChangesAsync();
            }
            else
            {
                var client = group.Clients.SingleOrDefault(_ => _.ClientId == ClientId);

                if (client == null)
                {

                    group.Clients.Add(new Domain.Entities.Client
                    {
                        ClientId = ClientId
                    });
                    await _instrumentRepository.SaveChangesAsync();
                }
            }

            await Groups.AddToGroupAsync(Context.ConnectionId, instrument);
        }

        public async Task UnSubscribeAsync(string instrument)
        {
            var group = await _instrumentRepository.GetBySpecAsync(new ClientsByProductGroupNameSpec(instrument));

            if(group == null)
            {
                throw new ArgumentNullException($"Client with Id: {ClientId} cannot unsubscribe from group that does not exist");
            }

            var client = await _clientRepository.GetByIdAsync(ClientId);

            if(client == null)
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
    }
}
