using Coinbase.Exchange.FrontEnd.ApiClient;
using Coinbase.Exchange.FrontEnd.Receivers;
using Microsoft.AspNetCore.Http.Connections;
using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.Extensions.Logging;

namespace Coinbase.Exchange.FrontEnd
{
    public partial class HomeForm : Form
    {
        private readonly HubConnection _hubConnection;
        private readonly MarketTraderReceiver _marketDataReceiver;
        private readonly MarketDataApiClient _marketDataApiClient;
        private readonly List<string> _subcribed_instruments;
        private int _selected_instrumentIndex;

        public HomeForm()
        {
            InitializeComponent();
            _marketDataReceiver = new MarketTraderReceiver();
            _marketDataApiClient = new MarketDataApiClient();
            _subcribed_instruments = new List<string>();
            _selected_instrumentIndex = 0;
            _hubConnection = new HubConnectionBuilder()
                                  .WithUrl("http://127.0.0.1:5190/exchangesubscription", options =>
                                  {
                                      options.Transports = HttpTransportType.WebSockets;
                                      options.AccessTokenProvider = _marketDataApiClient.GetAccessToken;
                                      options.SkipNegotiation = true;
                                  })
                                  .Build();

            _hubConnection.Closed += HubConnection_Closed;
            MarketTraderReceiver.OnMarketDataUpdate += MarketTraderReceiver_OnMarketDataUpdate;
        }

        private void MarketTraderReceiver_OnMarketDataUpdate(object? sender, MarketDataEventArgs e)
        {
            if (_subcribed_instruments.Any())
            {
                var selected_instrument = _subcribed_instruments[_selected_instrumentIndex];

                var store = e.Store;

                if (store.ContainsKey(selected_instrument))
                {
                    var bids = store[selected_instrument].Bids;
                    var offers = store[selected_instrument].Offers;
                    var bestbid = store[selected_instrument].BestBid;
                    var bestoffer = store[selected_instrument].BestOffer;
                    var price = store[selected_instrument].Price;

                    if (bids != null && bids.Any())
                    {
                        listBox_bids.Invoke(() =>
                        {
                            listBox_bids.Items.Clear();
                            listBox_bids.Items.Add("PRICE  |  QUANTITY");
                            listBox_bids.Items.AddRange(bids.Split('\n'));
                        });
                    }


                    if (offers != null && offers.Any())
                    {
                        listBox_offers.Invoke(() =>
                        {
                            listBox_offers.Items.Clear();
                            listBox_offers.Items.Add("PRICE  |  QUANTITY");
                            listBox_offers.Items.AddRange(offers.Split('\n'));
                        });
                    }


                    label_bestbid_value.Invoke(() =>
                    {
                        label_bestbid_value.Text = bestbid;
                    });

                    label_bestoffer_value.Invoke(() =>
                    {
                        label_bestoffer_value.Text = bestoffer;
                    });

                    label_price_value.Invoke(() =>
                    {
                        label_price_value.Text = price;
                    });

                }
            }
        }

        private async Task HubConnection_Closed(Exception? exception)
        {
            await Task.Delay(new Random().Next(0, 5) * 1000);
            try
            {
                await _hubConnection.StartAsync();
            }
            catch (Exception ex)
            {

                throw;
            }
        }

        private async void HomeForm_Load(object sender, EventArgs e)
        {
            _hubConnection.On<string, string, string>("receiveOrderBookUpdate", (instrument, type, data) =>
            {
                _marketDataReceiver.OnReceiveOrderBookMarketData(instrument, type, data);
            });

            try
            {
                var subscribed_instruments = await _marketDataApiClient.GetSubscribedInstruments();

                if (subscribed_instruments.Any())
                {
                    _subcribed_instruments.AddRange(subscribed_instruments);
                    comboBox_instruments.Items.AddRange(_subcribed_instruments.ToArray());
                    comboBox_instruments.SelectedIndex = _selected_instrumentIndex;
                }
                await _hubConnection.StartAsync();
            }
            catch (Exception ex)
            {

                await HubConnection_Closed(ex);
            }
        }

        private void comboBox_instruments_SelectedIndexChanged(object sender, EventArgs e)
        {
            _selected_instrumentIndex = comboBox_instruments.SelectedIndex;
        }
    }
}