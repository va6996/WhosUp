using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Parse;
using Windows.Storage;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using Windows.Devices.Geolocation;
using System.Collections;
using System.Net.NetworkInformation;
using Windows.UI.Popups;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkID=390556

namespace WhosUp
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class Nearby : Page
    {
        public Nearby()
        {
            this.InitializeComponent();

        }

        /// <summary>
        /// Invoked when this page is about to be displayed in a Frame.
        /// </summary>
        /// <param name="e">Event data that describes how this page was reached.
        /// This parameter is typically used to configure the page.</param>
        protected async override void OnNavigatedTo(NavigationEventArgs e)
        {
            bool isConnected = NetworkInterface.GetIsNetworkAvailable();
            if (!isConnected)
            {
                await new MessageDialog("No internet connection is avaliable. The full functionality of the app isn't avaliable.").ShowAsync();
                Application.Current.Exit();
            }

            Geolocator geolocator = new Geolocator();
            Geoposition pos = await geolocator.GetGeopositionAsync();
            var geo = new ParseGeoPoint(pos.Coordinate.Point.Position.Latitude, pos.Coordinate.Point.Position.Longitude);
            try { 
            ParseQuery<ParseObject> query = ParseObject.GetQuery("Activities")
                .WhereWithinDistance("Location", geo, ParseGeoDistance.FromMiles(10)).OrderByDescending("Interested");
            IEnumerable<ParseObject> nearbyLocations = await query.FindAsync();
            List<Info> users = new List<Info>();
            foreach (var item in nearbyLocations)
            {
                Info variable = new Info();
                variable.Activity = item.Get<String>("Activity");
                variable.Interested = item.Get<int>("Interested");
                variable.ObjID = item.ObjectId;
                ParseGeoPoint point = item.Get<ParseGeoPoint>("Location");
                variable.Lat = point.Latitude;
                variable.Long = point.Longitude;
                users.Add(variable);

            }
            Activities.ItemsSource = users;
           
                
            }
            catch(Exception ex)
            {
                Heading.Text = ex.Message;
            }
            
            Windows.Phone.UI.Input.HardwareButtons.BackPressed += App.HardwareButtons_BackPressed;
        }
        protected override void OnNavigatingFrom(NavigatingCancelEventArgs e)
        {
            base.OnNavigatingFrom(e);
            Windows.Phone.UI.Input.HardwareButtons.BackPressed -= App.HardwareButtons_BackPressed;
        }

        private void AddActivity(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(AddActivity));
        }

        private void NavDetail(object sender, TappedRoutedEventArgs e)
        {
            Info data = Activities.SelectedItem as Info;
            Frame.Navigate(typeof(Detail), data);
        }

        private void AboutClick(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(About));
        }
    }
}
