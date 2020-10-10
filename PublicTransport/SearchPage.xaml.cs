using client_data_functions;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PublicTransport.View;
using Xamarin.Forms;
using Xamarin.Forms.Maps;
using Xamarin.Forms.Xaml;

namespace PublicTransport
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Search : ContentPage
    {
        public Search()
        {
            InitializeComponent();
        }

        private static void Get_station_search_results(string text,ref ObservableCollection<View.StationSearchResults> dest)
        {
            Globals.all_stations_fetch.Wait();
            foreach(List<string> row in Globals.all_stations) {
                if (row[0].ToUpper().Contains(text.ToUpper())) {
                    dest.Add(new View.StationSearchResults(row[0]));
                }
            }
        }

        private void SearchBar_SearchButtonPressed(object sender, EventArgs e)
        { 
            ObservableCollection<View.StationSearchResults> x = new ObservableCollection<View.StationSearchResults>();
            Get_station_search_results(Searching.Text,ref x);
            StationResults.ItemsSource = null;
            if(x != null)
            {
                
                Error.IsVisible = false;
                StationResults.ItemsSource = x;
                VehicleResults.IsVisible = false;
                StationResults.IsVisible = true;
            }
            else
            {
                
                StationResults.IsVisible = false;
                Error.IsVisible = true;

            }

        }
        private void FloatMenuItemTap_OnTapped(object sender, EventArgs e)
        {
            Navigation.PopAsync();
        }

        private void Station_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            ObservableCollection<List<string>> data = SQLInterface.Get_Search_results(((View.StationSearchResults)e.Item).Get_nume_Station());
            ObservableCollection<StationSearchResults> vehicle_list_results = new ObservableCollection<StationSearchResults>();
            foreach (List<string> row in data)
            {
               vehicle_list_results.Add(new View.StationSearchResults(row[0]));
            }
            VehicleResults.ItemsSource = vehicle_list_results;
            StationResults.IsVisible = false;
            VehicleResults.IsVisible = true;

        }

        private void Vehicle_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            Globals.vname = ((View.StationSearchResults)e.Item).Get_nume_Station();
            Navigation.PushAsync(new PageMap());
            Globals.vehicle_update = true;
            Globals.Maps.Pins.Clear();
            Globals.updateServerTime = true;
            PageMap.Put_stations(Globals.Maps, Globals.vname);
            foreach (Pin pin in Globals.Listpins)
            {
                Globals.Maps.Pins.Add(pin);
            }
        }
        }
    }