using System;
using Windows.UI.Popups;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Parse;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.Storage;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Maps;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using Windows.Devices.Geolocation;
using System.Threading.Tasks;

namespace WhosUp
{

    public sealed partial class AddActivity : Page
    {
        public MapIcon pin;
        public AddActivity()
        {
            this.InitializeComponent();
        }

        async protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            Geolocator geo = new Geolocator();
            Geoposition pos = await geo.GetGeopositionAsync();
            BasicGeoposition upos = new BasicGeoposition() { Latitude = pos.Coordinate.Point.Position.Latitude, Longitude = pos.Coordinate.Point.Position.Longitude };
            MapSet.Center = new Geopoint(upos);
            MapSet.ZoomLevel = 15;
            MapSet.LandmarksVisible = true;
            pin = new MapIcon();
            pin.Location = new Geopoint(upos);
            pin.NormalizedAnchorPoint = new Point(0.5, 1.0);
            MapSet.MapElements.Add(pin); 
            MapSet.MapTapped += MapSet_MapTapped;
            Windows.Phone.UI.Input.HardwareButtons.BackPressed += App.HardwareButtons_BackPressed;
        }
        protected override void OnNavigatingFrom(NavigatingCancelEventArgs e)
        {
            base.OnNavigatingFrom(e);
            Windows.Phone.UI.Input.HardwareButtons.BackPressed -= App.HardwareButtons_BackPressed;
        }

        private void MapSet_MapTapped(MapControl sender, MapInputEventArgs args)
        {
            pin.Location = args.Location;
        }


        public async void SaveEvent(object sender, RoutedEventArgs e) 
        {
                try { 
                if(Activity.Text=="")
                {
                    InvalidInput();
                    return;
                }

                Info data = new Info()
                {
                    UserId = (++App.i).ToString(),
                    Activity = Activity.Text.Trim(),
                    Lat = pin.Location.Position.Latitude,
                    Long = pin.Location.Position.Longitude,
                    Interested = 1
            };
                var DataBase = new ParseObject("Activities");
                DataBase["UserID"] = data.UserId.ToString();
                DataBase["Interested"] = data.Interested;
                DataBase["Activity"] = data.Activity;
                DataBase["Location"] = new ParseGeoPoint(data.Lat, data.Long);
                await DataBase.SaveAsync();
                Frame.Navigate(typeof(Nearby));
            }
            catch(Exception ex)
            {
                Message.Text = ex.Message;
            }


        }

        private void Cancel(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(Nearby));
        }
        private async void InvalidInput()
        {   MessageDialog msgbox = new MessageDialog("Please try again!");
            await msgbox.ShowAsync();
        }
    }
}
