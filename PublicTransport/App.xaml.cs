﻿using RestEase;
using System.Threading.Tasks;
using Xamarin.Forms;


namespace PublicTransport
{

    public partial class App : Application
    {

        public App()
        {
            InitializeComponent();
            MainPage = new NavigationPage(new MenuPage());
        }

        protected override void OnStart()
        {
        }
        
        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }
    }
}
