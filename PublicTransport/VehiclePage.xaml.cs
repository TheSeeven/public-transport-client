using System;
using Xamarin.Forms;
using PublicTransport.View;
using Xamarin.Forms.Maps;
using client_data_functions;

namespace PublicTransport
{
    
    public partial class VehiclePage : ContentPage
    {
        

        public VehiclePage()
        {
            
            InitializeComponent();

            
            categories.ItemsSource = Globals.vehicle_type;
            categories.HeightRequest = (Globals.vehicle_type.Count * 80);
            
        }
        private void Categories_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            elements.IsVisible = true;
            elements.ItemsSource = View.Vehicles_list.Get_vehicles_list(((View.Type)e.Item).Get_nume_type());  
        }
        public void Elements_ItemTapped(object sender, SelectionChangedEventArgs e)
        {
            if (e.CurrentSelection.Count != 0)
            {
                Navigation.PushAsync(new PageMap());
                Globals.vehicle_update = true;
                Globals.vname = ((Vehicles_list)e.CurrentSelection[0]).Name;
                Globals.Maps.Pins.Clear();
                Globals.updateServerTime = true;
                PageMap.Put_stations(Globals.Maps, Globals.vname);
                foreach (Pin pin in Globals.Listpins)
                {
                    Globals.Maps.Pins.Add(pin);
                }
            ((CollectionView)sender).SelectedItem = null;
            }
        }
        private void FloatMenuItemTap_OnTapped(object sender, EventArgs e)
        {
            Navigation.PopAsync();
        }
    }
    
}