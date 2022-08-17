using LiveCharts;
using LiveCharts.Defaults;
using LiveCharts.Wpf;

using Microsoft.AspNetCore.SignalR.Client;

using Newtonsoft.Json;

using ServiceMonitoringTool.Wpf.Client.Models;
using ServiceMonitoringTool.Wpf.Client.Services;

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace ServiceMonitoringTool.Wpf.Client
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window, INotifyPropertyChanged
    {
        private readonly HubConnection connection;
        private SeriesCollection series = new SeriesCollection();
        private ObservableCollection<string> labels = new ObservableCollection<string>();
        private Func<double, string> formatter;
        private Func<DateTime, string> xFormatter;
        private readonly LiveChartDataService liveChartDataService;

        public MainWindow()
        {
            InitializeComponent();
            liveChartDataService = new LiveChartDataService();
            connection = new HubConnectionBuilder()
               .WithUrl("https://localhost:7169/MethodLogServiceHub")
               .WithAutomaticReconnect()
               .Build();

            connection.Closed += async (error) =>
            {
                await Task.Delay(new Random().Next(0, 5) * 1000);
                await connection.StartAsync();
            };

            this.Loaded += MainWindow_Loaded;

            DataContext = this;
        }
        private void ListBox_OnPreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            var item = ItemsControl.ContainerFromElement(ListBox, (DependencyObject)e.OriginalSource) as ListBoxItem;
            if (item == null)
                return;

            var series = (LineSeries)item.Content;
            series.Visibility = series.Visibility == Visibility.Visible
                ? Visibility.Hidden
                : Visibility.Visible;
        }
        private async void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {


            connection.On<string>("ChartPointReceived", (chartPoint) =>
            {
                this.Dispatcher.Invoke(() =>
                {
                    var jsonResponse = JsonConvert.DeserializeObject<ChartPointModel>(chartPoint);
                    if (jsonResponse is not null)
                        AddToChar(jsonResponse.ExecutionTime, jsonResponse.TimeElapsed.TotalSeconds, jsonResponse.Name + jsonResponse.ExecutionsStatus.ToString());
                });
            });

            connection.Reconnected += connectionId =>
            {
                Debug.Assert(connection.State == HubConnectionState.Connected);

                // Notify users the connection was reestablished.
                // Start dequeuing messages queued while reconnecting if any.
                MessageBox.Show($"Connection was reestablished with connection Id: {connectionId}");
                return Task.CompletedTask;
            };


            try
            {

                await connection.StartAsync();
                MessageBox.Show("Connection started");
                //var serverResponse = await liveChartDataService.GetLiveChartDataAsync("https://localhost:7169/queryservice", new Models.ServiceMonitorRequestModel { Id = "servicemonitoraggregate-43481fe0-3c7d-4a58-b07d-dd6a2480f7ab" });

                //if (serverResponse is not null)
                //{
                //    foreach (var seriesModel in serverResponse)
                //        if (seriesModel.Data is not null && seriesModel.Data.Any())
                //            foreach (var series in seriesModel.Data)
                //                AddToChar(series.Key, series.Value, seriesModel.Name);
                //}

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        public ObservableCollection<string> Labels { get => labels; set => SetProperty(ref labels, value); }
        public void AddToChar(DateTime executionTime, double timeElapsed, string serviceName)
        {
            if (Series is not null)
            {
                var chartValues = Series.FirstOrDefault(x => x.Title == serviceName);
                if (chartValues is not null)
                {
                    chartValues.Values.Add(timeElapsed);
                    //if (chartValues.Values.Count == 81)
                    //{
                    //    chartValues.Values.RemoveAt(0);
                    //    Labels.RemoveAt(0);
                    //}
                }
                else
                    Series.Add(new LineSeries
                    {
                        Values = new ChartValues<double> { timeElapsed },
                        Title = serviceName,
                        LineSmoothness = 0
                    });

                Labels.Add(FormartDatetime(executionTime));
            }


            YFormatter = value => value.ToString("N") + "Seconds";
            XFormatter = val => FormartDatetime(val);

            OnPropertyChanged(nameof(Series));
            OnPropertyChanged(nameof(Labels));
            OnPropertyChanged(nameof(XFormatter));
            OnPropertyChanged(nameof(YFormatter));
        }

        public string FormartDatetime(DateTime time)
        {
            if (time.Date == DateTime.Now.Date)
                // return just time
                return time.ToString("HH:mm");

            if (time.Date.Year == DateTime.Now.Year)
                // return just time
                return time.ToLocalTime().ToString("MMMM dd");

            // otherwise return a full date
            return time.ToLocalTime().ToString("dddd, dd MMMM yyyy");
        }
        public Func<DateTime, string> XFormatter { get => xFormatter; set => SetProperty(ref xFormatter, value); }
        public Func<double, string> YFormatter { get => formatter; set => SetProperty(ref formatter, value); }
        public SeriesCollection Series { get => series; set => SetProperty(ref series, value); }
        #region Notify Models
        public event PropertyChangedEventHandler PropertyChanged = (sender, e) => { };

        public void OnPropertyChanged([CallerMemberName] string name = default)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
        protected bool SetProperty<T>(ref T storage, T value, string[] dependentPropertyNames = null, [CallerMemberName] string propertyName = null, Action valueChanged = default)
        {
            if (Equals(storage, value))
                return false;

            storage = value;
            this.OnPropertyChanged(propertyName);

            if (dependentPropertyNames != null)
            {
                foreach (var dependentPropertyName in dependentPropertyNames)
                {
                    OnPropertyChanged(dependentPropertyName);
                }
            }

            if (valueChanged != default)
            {
                valueChanged.Invoke();
            }

            return true;
        }
        #endregion
    }

    public class ChartPointModel
    {
        public string Name { get; set; }

        public string Request { get; set; }

        public string Response { get; set; }

        public DateTime ExecutionTime { get; set; }

        public TimeSpan TimeElapsed { get; set; }

        public ExecutionsStatusType ExecutionsStatus { get; set; }

        public string ExecutedBy { get; set; }
    }
}
