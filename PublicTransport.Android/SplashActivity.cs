using System.Threading.Tasks;
using Android.App;
using Android.Content;
using Android.OS;


namespace PublicTransport.Droid
{
    [Activity(Label = "Transport",Theme = "@style/SplashTheme", MainLauncher = true, NoHistory = true )]
    class SplashActivity : Activity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

        }

        protected override void OnResume()
        {
            base.OnResume();
            Task startupwork = new Task(() => { SimulateStartup(); });
            startupwork.Start();
        }

        void SimulateStartup()
        {
            StartActivity(new Intent(Application.Context, typeof(MainActivity)));
        }
    }
}