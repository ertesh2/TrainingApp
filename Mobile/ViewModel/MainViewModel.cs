using System;
using System.ComponentModel;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;
using Windows.Devices.Geolocation;
using Newtonsoft.Json;
using TrainingApp.Model;
using TrainingApp.ViewModel;

namespace TrainingApp
{
    class MainViewModel : INotifyPropertyChanged
    {
        private string _latitude;
        private string _longitude;
        private string _statusText;
        private bool _isTracking;
        private LocationLogger _locationLogger;
        private Geolocator _locator;
        private Timer _timer;

        public event PropertyChangedEventHandler PropertyChanged;

        public MainViewModel()
        {
            _latitude = "?";
            _longitude = "?";
            _isTracking = false;
            _statusText = "Initializing...";
            _locationLogger = new LocationLogger();
            _locator = new Geolocator {DesiredAccuracy = PositionAccuracy.High};

            InitializeGps();
            
        }

        private async void InitializeGps()
        {
            var location = await _locator.GetGeopositionAsync().AsTask();
            Latitude = location.Coordinate.Point.Position.Latitude.ToString();
            Longitude = location.Coordinate.Point.Position.Longitude.ToString();
            if (!_isTracking)
            {
                StatusText = "GPS ready";
            }
        }

        public string StatusText
        {
            get { return _statusText; }
            set
            {
                _statusText = value;
                NotifyPropertyChanged("StatusText");
            }

        }

        public string Latitude
        {
            get { return _latitude; }
            set
            {
                _latitude = value;
                NotifyPropertyChanged("Latitude");
            }
        }

        public string Longitude
        {
            get { return _longitude; }
            set
            {
                _longitude = value;
                NotifyPropertyChanged("Longitude");
            }
        }

        public bool IsTracking
        {
            get { return _isTracking; }
            set
            {
                _isTracking = value;
                NotifyPropertyChanged("IsTracking");
            }
        }

        public ICommand ClickCommand
        {
            get { return new DelegateCommand(ClickHandler); }
        }

        private void ClickHandler()
        {
            if (!IsTracking)
            {
                IsTracking = true;
                _timer = new Timer(FindGpsLocation, null, TimeSpan.Zero, TimeSpan.FromSeconds(5));
                StatusText = "Tracking";
            }
            else
            {
                _timer.Dispose();
                IsTracking = false;
                StatusText = "Stopped";
            }
            TestWebService().ContinueWith((c) => { });
        }

        private async Task TestWebService() 
        {
            try
            {
                using (HttpClient client = new HttpClient())
                {
                    client.BaseAddress = new Uri("http://ertesh.azurewebsites.net/");

                    var url = "api/products/1";

                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                    HttpResponseMessage response = await client.GetAsync(url);

                    if (response.IsSuccessStatusCode)
                    {
                        var data = response.Content.ReadAsStringAsync();
                        var parsedData = JsonConvert.DeserializeObject<Product>(data.Result.ToString());
                        StatusText = parsedData.Name;
                    }
                }
            }
            catch (Exception ex)
            {
               // TODO:
            }
        }

        public void NotifyPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        private async void FindGpsLocation(object state)
        {
            var location = await _locator.GetGeopositionAsync().AsTask();
            _locationLogger.AddEntry(location);
        }
    }

    public class DelegateCommand : ICommand
    {
        private readonly Action _action;

        public DelegateCommand(Action action)
        {
            _action = action;
        }

        public void Execute(object parameter)
        {
            _action();
        }

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public event EventHandler CanExecuteChanged;
    }
}
