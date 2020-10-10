using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Xamarin.Forms.Maps;
using Xamarin.Essentials;
using client_data_functions;
using System.Threading.Tasks;
using System.Threading;

namespace PublicTransport
{
    class Globals
    {
        public static CustomMap Maps;
        public static bool update = true;
        public static bool vehicle_update = false;
        public static bool updateServerTime = false;
        public static string vname;
        public static int time = 3000;
        public static DateTime serverTime;
        public static List<Pin> Listpins = new List<Pin>();
        public static ObservableCollection<List<object>> Stations;
        public static ObservableCollection<List<string>> vehicles_list;
        public static ObservableCollection<View.Type> vehicle_type;
        public static ObservableCollection<List<string>> all_stations;

        public static Thread realtime_vehicle_location = new Thread(SQLInterface.Realtime_vehicle_location);
        public static Thread time_update = new Thread(SQLInterface.Time_updater);

        public static Task all_stations_fetch = new Task(()=> {
            SQLInterface.Get_all_stations();
        });
        public static Task type_vehicles_fetch = new Task(() => {
            vehicles_list = client_data_functions.SQLInterface.Get_Vehicle_list();
            vehicle_type = View.Type.Get_types();
        });

        


        public static bool Check_internet()
        {
            var current = Connectivity.NetworkAccess;
            if (current != NetworkAccess.Internet)
            {
                return false;
            }
            else return true;
        }
    
    }
}
