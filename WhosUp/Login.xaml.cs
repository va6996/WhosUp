using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Parse;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using System.Net.NetworkInformation;
using Windows.UI.Popups;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkID=390556

namespace WhosUp
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class Login : Page
    {
        public Login()
        {
            this.InitializeComponent();
            ParseFacebookUtils.Initialize("1142192829144111");

        }


        protected async override void OnNavigatedTo(NavigationEventArgs e)

        {
            bool isConnected = NetworkInterface.GetIsNetworkAvailable();
            if (!isConnected)
            {
                await new MessageDialog("No internet connection is avaliable. The full functionality of the app isn't avaliable.").ShowAsync();
                Application.Current.Exit();
            }
        }

        private void nav(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(Nearby));
        }

        private async void fb(object sender, RoutedEventArgs e)
        {
            browser.Visibility = Visibility.Visible;
            try
            {
                ParseUser user = await ParseFacebookUtils.LogInAsync(browser, null);
            }
            catch
            {
            }
            browser.Visibility = Visibility.Collapsed;
            Frame.Navigate(typeof(Nearby));
        }
    }
}
