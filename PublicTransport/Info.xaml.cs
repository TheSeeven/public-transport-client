using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace PublicTransport
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Info : ContentPage
    {
        public Info()
        {
            InitializeComponent();
            ((NavigationPage)Application.Current.MainPage).BarBackgroundColor = Color.FromHex("#323232");
            
        }
        private void FloatMenuItemTap_OnTapped(object sender, EventArgs e)
        {
            Navigation.PopAsync();
            Globals.vehicle_update = true;
        }
    }
}