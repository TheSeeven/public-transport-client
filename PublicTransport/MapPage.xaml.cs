using System;
using System.Collections.Generic;
using System.ComponentModel;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Maps;
using Plugin.Permissions;
using Plugin.Permissions.Abstractions;
using System.Collections.ObjectModel;
using client_data_functions;

namespace PublicTransport
{
    [DesignTimeVisible(false)]
    public partial class PageMap : ContentPage
    {
        public PageMap()
        {
            InitializeComponent();


            ((NavigationPage)Application.Current.MainPage).Popped += OnPoppedToRoot;
            ((NavigationPage)Application.Current.MainPage).BarBackgroundColor = Color.FromHex("#323232");
           
                //  Getting the permision to get your location and show it on the map   //
            GetPermissions();
            //  Setting the initial position of the map on Timisoara   //
            Maps.MoveToRegion(MapSpan.FromCenterAndRadius(new Position(45.756646, 21.228666), Distance.FromKilometers(3)));
            
            Globals.Maps = Maps;
        }
        private void OnPoppedToRoot(object sender, NavigationEventArgs e)
        {
            Globals.vehicle_update = false;
            Globals.updateServerTime = false;
        }
        public static void PinsV(string nume, double lat, double lng, bool problem)
        {
            if (problem) { nume += "1"; }
            else { nume += "0"; }

            CustomPin x = new CustomPin
                {
                    Label = nume,
                    Type = PinType.Place,
                    Position = new Position(lat, lng)
                };
                
            
            Globals.Maps.Pins.Add(x);
        }
        public static void Put_stations(Xamarin.Forms.Maps.Map Maps,string vname)
        {
            Globals.Stations = new ObservableCollection<List<object>>(SQLInterface.Get_stations(vname));
            
            foreach (List<object> row in Globals.Stations)
            {
                Pin statie = new Pin
                {
                    Label = row[1].ToString(),
                    Type = PinType.Generic,
                    Address = " ",
                    Position = new Position(Convert.ToDouble(row[2]), Convert.ToDouble(row[3]))
                };
                statie.MarkerClicked += (sender, e) =>
                {
                    Ad(Convert.ToUInt32(row[0]), (Pin)sender);
                };
                Maps.Pins.Add(statie);
            }
        }

        private static void Ad(uint st, Pin sender)
        {
            ObservableCollection<Vehicle> x = SQLInterface.Time_lists(st);
            string res = "";
            try
            {
                foreach (Vehicle i in x)
                {
                    if (!i.problem)
                    {
                        res += $"{i.name} : {i.time} || ";
                    }
                    else res += $"{i.name} : Indisponibil || ";
                }
                res = res.Remove(res.Length - 4);
            }
            catch { }
            finally
            {
                sender.Address = res;
            }
        }

        private async void GetPermissions()
        {
            try
            {
                #pragma warning disable 0618
                var status = await CrossPermissions.Current.CheckPermissionStatusAsync(Permission.Location);
                if (status != Plugin.Permissions.Abstractions.PermissionStatus.Granted)
                {
                    //  Asking the user for permision to get their location  //
                    if (await CrossPermissions.Current.ShouldShowRequestPermissionRationaleAsync(Permission.Location))
                    {
                        await DisplayAlert("Location Request", "Your location needs to be accesed", "OK");
                    }

                    var results = await CrossPermissions.Current.RequestPermissionsAsync(Permission.Location);
                    status = results[Permission.Location];
                }
                if (status == Plugin.Permissions.Abstractions.PermissionStatus.Granted)
                {
                    //  Setting the map to show the user's location if they accepted  //
                    Maps.IsShowingUser = true;
                }
                    //  Show alert if the user didn't accept the request  //
                else { await DisplayAlert("Location Denied", "We can't show your lovation in map", "OK"); }
            }
            catch 
            { }
        }
       
        
            //  Economy mode button functions   //
        private void FloatMenuItem2Tap_OnTapped(object sender, EventArgs e)
        {
            if (Preferences.Get("Economy", false) == false)
                {
                    ((Frame)sender).BackgroundColor = Color.DarkGreen;
                    Globals.time = 7000;
                    Preferences.Set("Economy", true);
                }
                else
                {
                    Globals.time = 3000;
                    ((Frame)sender).BackgroundColor = Color.FromHex("#323232");
                    
                    Preferences.Set("Economy", false);    
                }
        }
           //  Information page button  //
        private void FloatMenuItem3Tap_OnTapped(object sender, EventArgs e)
        {
            //  Takes you to the Information page  //
            Navigation.PushAsync(new Info());
            Globals.vehicle_update = false;
            
        }
        private void FloatMenuItemTap_OnTapped1(object sender, EventArgs e)
        {
            Navigation.PopAsync();
        }

    }
}
