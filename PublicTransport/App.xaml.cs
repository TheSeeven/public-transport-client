using RestEase;
using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
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

            //int x = 10;
            //Execute_querry("11:get_types[]");
            //Console.WriteLine(x);
        }

        //public static string Execute_querry(string q)
        //{
        //    int size_i = 1;
        //    string size = "";
        //    string rez;
        //    bool got_size = false;
        //    byte[] bytes = new byte[size_i];

        //    Socket client = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        //    IPEndPoint ip = new IPEndPoint(IPAddress.Parse("192.168.1.110"),3301);
        //    client.Connect(ip);
        //    client.Send(System.Text.Encoding.ASCII.GetBytes(q));

        //    while(true)
        //    {
        //        client.Receive(bytes);
        //        rez = Encoding.ASCII.GetString(bytes, 0, size_i);
        //        if (!got_size)
        //        {    
        //            try
        //            {
        //                Convert.ToInt32(rez);
        //                size += rez;
        //                continue;
        //            }
        //            catch
        //            {
        //                size_i = Convert.ToInt32(size);
        //                got_size = true;
        //                bytes = new byte[size_i];
        //                continue;
        //            }
        //        }
        //        client.Close();
        //        break;
        //    }
        //    return rez;
        //}

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
