
using System;
using System.ComponentModel;
using Windows.Devices.Geolocation;
using Windows.Foundation;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Maps;
using Windows.UI.Xaml.Navigation;

namespace TrainingApp
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            this.InitializeComponent();

            this.NavigationCacheMode = NavigationCacheMode.Required;
        }

        private MainViewModel _viewModel;

        /// <summary>
        /// Invoked when this page is about to be displayed in a Frame.
        /// </summary>
        /// <param name="e">Event data that describes how this page was reached.
        /// This parameter is typically used to configure the page.</param>
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            // TODO: Prepare page for display here.

            // TODO: If your application contains multiple pages, ensure that you are
            // handling the hardware Back button by registering for the
            // Windows.Phone.UI.Input.HardwareButtons.BackPressed event.
            // If you are using the NavigationHelper provided by some templates,
            // this event is handled for you.

            _viewModel = new MainViewModel();
            _viewModel.PropertyChanged += ViewModelOnPropertyChanged;
            this.DataContext = _viewModel;

        }

        private void ViewModelOnPropertyChanged(object sender, PropertyChangedEventArgs args)
        {
            if ((args.PropertyName == "Latitude") || (args.PropertyName == "Longitude"))
            {
                try
                {
                    var MapIcon1 = new MapIcon();
                    var location = new Geopoint(new BasicGeoposition()
                    {
                        Latitude = Convert.ToDouble(_viewModel.Latitude),
                        Longitude = Convert.ToDouble(_viewModel.Longitude)
                    });
                    MapIcon1.Location = location;
                    MapIcon1.NormalizedAnchorPoint = new Point(0.5, 1.0);
                    MapIcon1.Title = "Location";
                    //this.MapPreview.Center = location;
                    //this.MapPreview.ZoomLevel = 12;
                    //this.MapPreview.MapElements.Add(MapIcon1);
                }
                catch (Exception)
                {
                    
                }
            }
        }
    }
}
