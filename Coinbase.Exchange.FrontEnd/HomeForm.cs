using Coinbase.Exchange.FrontEnd.ApiClient;
using Coinbase.Exchange.FrontEnd.Receivers;
using Coinbase.Exchange.SharedKernel.Models.Account;
using Coinbase.Exchange.SharedKernel.Models.ApiDto;
using Coinbase.Exchange.SharedKernel.Models.Products;
using Coinbase.Exchange.SharedKernel.Models.Subscription;
using Microsoft.AspNetCore.Http.Connections;
using Microsoft.AspNetCore.SignalR.Client;
using System.Globalization;
using System.Windows.Forms.DataVisualization.Charting;

namespace Coinbase.Exchange.FrontEnd
{
    public partial class HomeForm : Form
    {
        private readonly HubConnection _hubConnection;
        private readonly MarketTraderReceiver _marketDataReceiver;
        private readonly MarketDataApiClient _marketDataApiClient;
        private List<string> _subcribed_instruments;
        private int _selected_instrumentIndex;
        private int _selected_accountIndex;
        private Dictionary<string, Product> _all_Instruments;
        private List<string> _instruments;
        private List<AccountEntry> _accounts;
        private List<CandleDetails> _candles;

        public HomeForm(MarketTraderReceiver marketTraderReceiver,
            MarketDataApiClient marketDataApiClient)
        {
            InitializeComponent();
            _marketDataReceiver = marketTraderReceiver;
            _marketDataApiClient = marketDataApiClient;
            _subcribed_instruments = new List<string>();
            _selected_instrumentIndex = 0;
            _selected_accountIndex = 0;
            _candles = new List<CandleDetails>();
            _hubConnection = new HubConnectionBuilder()
                                  .WithUrl("http://127.0.0.1:5190/exchangesubscription", options =>
                                  {
                                      options.Transports = HttpTransportType.WebSockets;
                                      options.AccessTokenProvider = _marketDataApiClient.GetAccessToken;
                                      options.SkipNegotiation = true;
                                  })
                                  .WithAutomaticReconnect()
                                  .Build();

            _hubConnection.Closed += HubConnection_Closed;
            DataReceiver.OnMarketDataUpdate += MarketTraderReceiver_OnMarketDataUpdate;
        }

        private void MarketTraderReceiver_OnMarketDataUpdate(object? sender, MarketDataEventArgs e)
        {
            _selected_instrumentIndex = _selected_instrumentIndex == -1 ? 0 : _selected_instrumentIndex;
            if (_subcribed_instruments.Any())
            {
                var selected_instrument = _subcribed_instruments[_selected_instrumentIndex];

                var store = e.Store;

                if (store.ContainsKey(selected_instrument))
                {
                    var bids = store[selected_instrument].Bids;
                    var offers = store[selected_instrument].Offers;
                    var price = store[selected_instrument].Price;
                    var candles = store[selected_instrument].Candles;
                    UpdatePriceData(price);
                    UpdateBidsData(bids);
                    UpdateOffersData(offers);
                    UpdateCandleChartData(candles);
                }
                else
                {
                    this.Invoke(() =>
                    {
                        ClearDisplay();
                    });

                }
            }
        }

        private void UpdateCandleChartData(List<CandleDetails> candles)
        {
            if (candles != null && candles.Any())
            {
                _candles = candles;
                chart_candles.Invoke(() =>
                {
                    Load_Candle_Stick_data();
                });
            }
        }

        private void UpdateOffersData(List<OrderBookUpdate> offers)
        {
            if (offers != null && offers.Any())
            {
                var bestOffer = offers[0].Price.ToString();
                var source = new BindingSource();
                source.DataSource = offers;
                label_bestoffer_value.Invoke(() =>
                {
                    label_bestoffer_value.Refresh();
                    label_bestoffer_value.Text = bestOffer;
                    dataGridView_offers.AutoGenerateColumns = true;
                    dataGridView_offers.DataSource = source;
                });
            }
        }

        private void UpdateBidsData(List<OrderBookUpdate> bids)
        {
            if (bids != null && bids.Any())
            {
                var bestbid = bids[0].Price.ToString();
                var source = new BindingSource();
                source.DataSource = bids;
                label_bestbid_value.Invoke(() =>
                {
                    label_bestbid_value.Refresh();
                    label_bestbid_value.Text = bestbid;
                    dataGridView_bids.AutoGenerateColumns = true;
                    dataGridView_bids.DataSource = source;
                });
            }
        }

        private void UpdatePriceData(decimal price)
        {
            label_price_value.Invoke(() =>
            {
                label_price_value.Refresh();
                label_price_value.Text = price.ToString();
            });
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
                MessageBox.Show("Error connecting to SignalR hub", "Connection Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private async void HomeForm_Load(object sender, EventArgs e)
        {
            _hubConnection.On<string, string, string>("MarketDataResultUpdate", (instrument, type, data) =>
            {
                _marketDataReceiver.OnReceiveOrderBookMarketData(instrument, type, data);
            });

            try
            {
                var subscribed_instruments = await _marketDataApiClient.GetSubscribedInstruments();
                _all_Instruments = await _marketDataApiClient.GetAllTradedInstruments();
                _accounts = await _marketDataApiClient.GetAllAccounts();

                if (subscribed_instruments.Any())
                {
                    _subcribed_instruments.AddRange(subscribed_instruments);
                    comboBox_instruments.DataSource = _subcribed_instruments;
                    comboBox_instruments.SelectedIndex = _selected_instrumentIndex;
                    listBox_subscribed_instruments.DataSource = _subcribed_instruments.ToList();
                }
                if (_all_Instruments.Any())
                {
                    _instruments = _all_Instruments.Keys.Except(subscribed_instruments).ToList();
                    listBox_instruments.DataSource = _instruments;
                }
                if (_accounts.Any())
                {
                    comboBox_accounts.DataSource = _accounts.Select(_ => _.Name).ToList();
                    comboBox_accounts.SelectedIndex = _selected_accountIndex;
                    label_balance_value.Text = $"{Math.Round(decimal.Parse(_accounts[_selected_accountIndex].Available_Balance.Value, CultureInfo.InvariantCulture), 4)} " +
                        $"{_accounts[_selected_accountIndex].Available_Balance.Currency}";
                }
                await _hubConnection.StartAsync();

                Load_Candle_Stick_data();
                if (!_subcribed_instruments.Any())
                    button_remove_instrument.Enabled = false;
                else
                    button_remove_instrument.Enabled = true;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error connecting to Market Data Server Feed", "Connection Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                await HubConnection_Closed(ex);
            }
        }

        private void comboBox_instruments_SelectedIndexChanged(object sender, EventArgs e)
        {
            _selected_instrumentIndex = comboBox_instruments.SelectedIndex;
            //ClearDisplay();
        }

        private void ClearDisplay()
        {
            label_bestbid_value.Refresh();
            label_bestoffer_value.Refresh();
            dataGridView_bids.Refresh();
            label_price_value.Refresh();
            dataGridView_offers.Refresh();

            label_bestbid_value.Text = string.Empty;
            label_bestoffer_value.Text = string.Empty;
            label_price_value.Text = string.Empty;
            dataGridView_bids.DataSource = default;
            dataGridView_offers.DataSource = default;
            ClearChart();
        }

        private void ClearChart()
        {
            foreach (var series in chart_candles.Series)
            {
                series.Points.Clear();
            };
            chart_candles.Titles.Clear();
            chart_candles.DataSource = default;
            chart_candles.DataBind();
        }

        private async void button_add_instrument_Click(object sender, EventArgs e)
        {
            try
            {
                
                var instruments = listBox_instruments.SelectedItems.Cast<string>().ToList();

                await _hubConnection.InvokeAsync("SubscribeMultipleAsync", instruments);

                _subcribed_instruments.AddRange(instruments);
                _instruments = _instruments.Except(instruments).ToList();

                comboBox_instruments.DataSource = null;
                comboBox_instruments.DataSource = _subcribed_instruments;
                listBox_instruments.DataSource = _instruments;
                listBox_subscribed_instruments.DataSource = _subcribed_instruments.ToList();
                _selected_instrumentIndex = 0;
                comboBox_instruments.SelectedIndex = _selected_instrumentIndex;
                if (_subcribed_instruments.Any())
                    button_remove_instrument.Enabled = true;

                MessageBox.Show($"Successfully subscribed to the following: {string.Join(",", instruments)} feed",
                   "Instrument Subscription", MessageBoxButtons.OK, MessageBoxIcon.Information);

                
            }
            catch (Exception)
            {

                MessageBox.Show("Error subscribing to instrument feed", "Subscription Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private async void button_remove_instrument_Click(object sender, EventArgs e)
        {
            try
            {
                
                var instruments = listBox_subscribed_instruments.SelectedItems.Cast<string>().ToList();

                await _hubConnection.InvokeAsync("UnSubscribeMultipleAsync", instruments);

                _subcribed_instruments = _subcribed_instruments.Except(instruments).ToList();
                _instruments = _instruments.Concat(instruments).ToList();
                comboBox_instruments.DataSource = null;
                comboBox_instruments.DataSource = _subcribed_instruments;
                listBox_instruments.DataSource = _instruments;
                listBox_subscribed_instruments.DataSource = _subcribed_instruments.ToList();
                _selected_instrumentIndex = 0;

               

                MessageBox.Show($"Successfully unsubscribed to the following: {string.Join(",", instruments)} feed",
                    "Instrument Subscription", MessageBoxButtons.OK, MessageBoxIcon.Information);

                if (!_subcribed_instruments.Any())
                {
                    button_remove_instrument.Enabled = false;
                    ClearDisplay();
                }
                else
                {

                    comboBox_instruments.SelectedIndex = _selected_instrumentIndex;
                }



            }
            catch (Exception)
            {
                MessageBox.Show("Error subscribing to instrument feed", "Subscription Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void label_instruments_Click(object sender, EventArgs e)
        {

        }

        private async void comboBox_accounts_SelectedIndexChanged(object sender, EventArgs e)
        {
            _selected_accountIndex = comboBox_accounts.SelectedIndex;
            

            var balance = await _marketDataApiClient.GetAccounts(_accounts[_selected_accountIndex].Uuid);

            if(balance != null)
            {
                _accounts[_selected_accountIndex] = balance;
                
            }
            label_balance_value.Text = $"{Math.Round(decimal.Parse(_accounts[_selected_accountIndex].Available_Balance.Value, CultureInfo.InvariantCulture), 4)} " +
                        $"{_accounts[_selected_accountIndex].Available_Balance.Currency}";

        }

        private void Load_Candle_Stick_data()
        {
            if (_candles.Any())
            {
                ClearChart();
                
                chart_candles.ChartAreas["ChartArea1"].AxisX.MajorGrid.LineWidth = 0;
                chart_candles.ChartAreas["ChartArea1"].AxisY.MajorGrid.LineWidth = 0;

                var candle_soource = _candles.Skip(Math.Max(0, _candles.Count() - 100)).ToList();

                var min = candle_soource.Min(_ => _.Low);
                var max = candle_soource.Max(_ => _.High);

                var range = (max - min) / 2;

                var interval = (max - min) / 4;

                var candle = candle_soource.Last();

                


                var label = $"O{candle.Open} H{candle.High} L{candle.Low} C{candle.Close} V{candle.Volume}";

                chart_candles.Series["Volume"].XValueMember = "Time";
                chart_candles.Series["Volume"].YValueMembers = "Open,High,Low,Close";
                chart_candles.Series["Volume"].ChartType = SeriesChartType.Candlestick;
                chart_candles.Series["Volume"]["PointWidth"] = "0.2";
                chart_candles.ChartAreas[0].AxisX.Interval = 20d;
                chart_candles.ChartAreas[0].AxisY.Interval = (double)(interval);
                chart_candles.ChartAreas[0].AxisY.Maximum = (double)(max + range);
                chart_candles.ChartAreas[0].AxisY.Minimum = (double)(min - range);
                chart_candles.Series["Volume"].XValueType = System.Windows.Forms.DataVisualization.Charting.ChartValueType.Double;
                chart_candles.Series["Volume"].CustomProperties = "PriceDownColor=Red,PriceUpColor=Green";
                chart_candles.Series["Volume"]["ShowOpenClose"] = "Both";
                chart_candles.Titles.Add(label);
                chart_candles.DataManipulator.IsStartFromFirst = true;
                chart_candles.DataSource = default;
                var source = new BindingSource();
                source.DataSource = candle_soource;

                chart_candles.DataSource = source;
                chart_candles.DataBind();

            }
        }

        private void label_add_subscription_Click(object sender, EventArgs e)
        {

        }

        private void chart_candles_Click(object sender, EventArgs e)
        {

        }
    }
}