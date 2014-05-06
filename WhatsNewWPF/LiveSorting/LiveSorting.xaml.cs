using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Threading;

namespace WhatsNewWPF.LiveSorting
{
    public partial class LiveSortingPage : Page
    {
        public LiveSortingPage()
        {
            InitializeComponent();
            this.Loaded += OnLoaded;
        }

        private void OnLoaded(object sender, RoutedEventArgs routedEventArgs)
        {
            var source = Resources["StockData"] as CollectionViewSource;
            if (source == null) return;

            var view = source.View as ICollectionViewLiveShaping;
            if (view != null && view.CanChangeLiveSorting)
            {
                source.SortDescriptions.Add(new SortDescription("Price", ListSortDirection.Descending));                
                view.IsLiveSorting = true;
                view.LiveSortingProperties.Add("Price");
            }
        }

        private void Activate_OnClick(object sender, RoutedEventArgs e)
        {
            var vm = Resources["ViewModel"] as LiveSortingViewModel;
            if (vm == null) return;
            if (Activate.Content.ToString() == "Stop")
            {
                vm.StopUpdates();
                Activate.Content = "Activate";
            }
            else
            {
                vm.RandomlyUpdate();
                Activate.Content = "Stop";
            }
        }
    }

    public class LiveSortingViewModel
    {
        private Random rand = new Random();
        private DispatcherTimer _timer;
        public ObservableCollection<StockTicker> Stocks { get; set; }

        public LiveSortingViewModel()
        {
            Stocks = new ObservableCollection<StockTicker>()
                {
                    new StockTicker() { Name="Company AAA", Symbol = "AAA", Price = rand.NextDouble() * 100, ChangeDirection = GetDirection()},
                    new StockTicker() { Name="Company BBB", Symbol = "BBB", Price = rand.NextDouble() * 100, ChangeDirection = GetDirection()},
                    new StockTicker() { Name="Company CCC", Symbol = "CCC", Price = rand.NextDouble() * 100, ChangeDirection = GetDirection()},
                    new StockTicker() { Name="Company DDD", Symbol = "DDD", Price = rand.NextDouble() * 100, ChangeDirection = GetDirection()},
                    new StockTicker() { Name="Company EEE", Symbol = "EEE", Price = rand.NextDouble() * 100, ChangeDirection = GetDirection()}
                };

            _timer = new DispatcherTimer(DispatcherPriority.Normal);
            _timer.Tick += UpdateStocks;
            _timer.Interval = TimeSpan.FromMilliseconds(500);
        }

        private void UpdateStocks(object sender, EventArgs e)
        {
            var stocksToUpdate = Stocks.Where(s => rand.NextDouble() < 0.5);
            foreach (var stock in stocksToUpdate)
            {
                var oldPrice = stock.Price;
                var newPrice = oldPrice + rand.NextDouble() * 20 - 10;
                stock.Price = Math.Max(newPrice, 0);
                stock.ChangeDirection = GetDirection(oldPrice, stock.Price);
            }
        }

        public ChangeDirection GetDirection(double oldPrice = 0d, double newPrice = 0d)
        {
            if (oldPrice == 0d && newPrice == 0d)
            {
                return rand.NextDouble() >= 0.5d ? ChangeDirection.Up : ChangeDirection.Down;
            }
            if (Math.Abs(oldPrice - newPrice) < .01)
            {
                return ChangeDirection.Neutral;
            }
            return oldPrice < newPrice ? ChangeDirection.Up : ChangeDirection.Down;
        }

        public void RandomlyUpdate()
        {
            _timer.Start();
        }

        public void StopUpdates()
        {
            _timer.Stop();
        }
    }

    public class StockTicker : INotifyPropertyChanged
    {
        private double _price;
        private ChangeDirection _changeDirection;

        public event PropertyChangedEventHandler PropertyChanged;

        public string Name { get; set; }
        public string Symbol { get; set; }

        public ChangeDirection ChangeDirection
        {
            get { return _changeDirection; }
            set
            {
                _changeDirection = value;
                OnPropertyChanged("ChangeDirection");
            }
        }

        public double Price
        {
            get { return _price; }
            set
            {
                _price = value;
                OnPropertyChanged("Price");
            }
        }

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            var handler = PropertyChanged;
            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }

    public enum ChangeDirection
    {
        Up,
        Down,
        Neutral
    }
}
