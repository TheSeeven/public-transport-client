using System;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace PublicTransport
{
    [XamlCompilation(XamlCompilationOptions.Compile)]

   
    public partial class MenuPage : ContentPage
    {
        public MenuPage()
        {
            InitializeComponent();
            // to add handler when resume from background
            if (!Globals.realtime_vehicle_location.IsAlive) {
                Globals.realtime_vehicle_location.Start();
                Globals.time_update.Start();
            }
            if (Preferences.Get("Economy", false))
            {
                Globals.time = 6000;
            }
            else Globals.time = 1500;
        }
        public void Vehicle_tap(object sender, EventArgs e)
        {
            if (!Globals.type_vehicles_fetch.IsCompleted & Globals.vehicles_list == null & Globals.vehicle_type == null)
            {
                Globals.type_vehicles_fetch.RunSynchronously();
            }
            Navigation.PushAsync(new VehiclePage());
        }

        public void Search_tap(object sender, EventArgs e)
        {
            if(!Globals.all_stations_fetch.IsCompleted & Globals.all_stations==null)
            {
                Globals.all_stations_fetch.RunSynchronously();
            }
            if (!Globals.type_vehicles_fetch.IsCompleted & Globals.vehicles_list == null & Globals.vehicle_type == null)
            {
                Globals.type_vehicles_fetch.RunSynchronously();
            }

            Navigation.PushAsync(new Search());
        }

        public void Info_tap(object sender, EventArgs e)
        {
            Navigation.PushAsync(new Info());
        }

        //  Economy mode button functions   //
        private void FloatMenuItem2Tap_OnTapped(object sender, EventArgs e)
        {
            if (Preferences.Get("Economy", false) == false)
            {
                ((Frame)sender).BackgroundColor = Color.DarkGreen;
                Globals.time = 6000;
                Preferences.Set("Economy", true);
            }
            else
            {
                Globals.time = 1500;
                ((Frame)sender).BackgroundColor = Color.FromHex("#323232");

                Preferences.Set("Economy", false);
            }
        }
    }
}