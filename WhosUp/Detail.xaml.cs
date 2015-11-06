using System;
using System.Collections.Generic;
using System.IO;
using Parse;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Devices.Geolocation;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Maps;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using System.Collections;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkID=390556

namespace WhosUp
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class Detail : Page
    {
        Info info;
        public Detail()
        {
            this.InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            info = e.Parameter as Info;
            ActivityName.Text = info.Activity;
            UpFor.Text = info.Interested.ToString() + " people are up for this!";
            BasicGeoposition upos = new BasicGeoposition() { Latitude = info.Lat, Longitude = info.Long };
            Map.Center = new Geopoint(upos);
            Map.ZoomLevel = 15;
            Map.LandmarksVisible = true;
            MapIcon pin = new MapIcon();
            pin.Location = new Geopoint(upos);
            pin.NormalizedAnchorPoint = new Point(0.5, 1.0);
            Map.MapElements.Add(pin);
            Windows.Phone.UI.Input.HardwareButtons.BackPressed += App.HardwareButtons_BackPressed;

        }
        protected override void OnNavigatingFrom(NavigatingCancelEventArgs e)
        {
            base.OnNavigatingFrom(e);
            Windows.Phone.UI.Input.HardwareButtons.BackPressed -= App.HardwareButtons_BackPressed;
        }

        private void NotIncrement(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(Nearby));
        }

        private async void Increment(object sender, RoutedEventArgs e)
        {
            ParseObject obj = await ParseObject.GetQuery("Activities").GetAsync(info.ObjID);
            obj.Increment("Interested");
            UpFor.Text = (info.Interested + 1).ToString() + " people are up for this!";
            await obj.SaveAsync();
            Check.Visibility = Visibility.Collapsed;
        }
    }
}
