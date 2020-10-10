using RestEase;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading;
using System.Threading.Tasks;
using PublicTransport;
using Xamarin.Forms.Maps;

namespace client_data_functions
{
    interface IExecuteQuerry
    {
        [Get("execute_querry/{q}")]
        Task<string> GetResult([Path] string q);
    }
    public class SQLInterface
    {
        
        private static readonly IExecuteQuerry client = RestClient.For<IExecuteQuerry>("http://192.168.1.110:3301");
        private static void Execute_querry(string querry, ref string dest) // Executes a querry and assigns the result for a given querry (querry is self made) proc_name[arguments_list, sep=;] to ref variable 
        {
            try
            {dest = client.GetResult(querry).Result;}
            catch
            {Execute_querry(querry, ref dest);}
        }
        private static List<List<string>> Parse_MSG(string MSG) // Transform the string into a matrix of string of data
        {
            List<List<string>> rez = new List<List<string>>();
            List<string> row = new List<string>();
            string str_temp = "";
            foreach (char i in MSG)
            {
                if (i == '$')
                {
                    row.Add(str_temp);
                    rez.Add(new List<string>(row));
                    break;
                }
                else if (i == ';')
                {
                    row.Add(str_temp);
                    rez.Add(new List<string>(row));
                    str_temp = "";
                    row.Clear();
                    continue;
                }
                else if (i == ',')
                {
                    row.Add(str_temp);
                    str_temp = "";
                    continue;
                }
                else
                {
                    str_temp += i;
                }
            }
            return rez;
        }
        
        private static int Get_tod() // Returns the current TOD 
        {
            int current = (DateTime.Now.TimeOfDay).Hours;
            if (current <= 10 & current > 0) { return 1; }
            else if (current <= 14 & current > 10) { return 2; }
            else if (current <= 18 & current > 14) { return 3; }
            else if (current <= 23 & current > 18) { return 4; }
            else return 1;
        }
        private static ObservableCollection<List<object>> Path_table(uint station_id) // Returns a table of data used for calculating the arrival time for vehicles inside the same table.
        {
            ObservableCollection<List<object>> rez = new ObservableCollection<List<object>>();
            string data_string = "";
            Execute_querry($"time_table[{station_id}]", ref data_string);
            List<List<string>> data = new List<List<string>>(Parse_MSG(data_string));
            foreach (List<string> row in data)
            {
                try
                {
                    rez.Add(new List<object>(new object[] { row[0], Convert.ToUInt32(row[1]), Convert.ToUInt32(row[2]), Convert.ToUInt32(row[3]), Convert.ToUInt32(row[4]), Convert.ToUInt32(row[5]), Convert.ToBoolean(Convert.ToInt32(row[6])), Convert.ToInt32(row[7]), Convert.ToDateTime(Convert.ToString(row[8])) }));
                }
                catch
                {
                    return rez;
                }
            }


            return rez;
        }

        private static bool Is_station(double lat, double lng)
        {
            foreach (List<object> sta in Globals.Stations)
            {
                if (Convert.ToDouble(sta[2]) == lat && Convert.ToDouble(sta[3]) == lng)
                    return true;
            }
            return false;
        }

        // Below are the function to access the data by using a given broker. The broker IP is HARDCODED along with the rest of connection info

        public static ObservableCollection<Vehicle> Time_lists(uint station_id) // Return a list of vehicles with their time for coming in a specific station
        {
            List<List<object>> data_table = new List<List<object>>(Path_table(station_id));
            List<Vehicle> time_result = new List<Vehicle>();

            static int get_nr_stations(string vehicle)
            {
                string data_string = "";
                Execute_querry($"get_station_nr[{vehicle}]",ref data_string);
                return Convert.ToInt32(Parse_MSG(data_string));
            }

            bool found = false;
            for (int i = 0; i < data_table.Count; i += 2){
                foreach (Vehicle vehicle in time_result){
                    if (vehicle.name == (string)(data_table[i][0])){
                        found = true;
                        break;
                    }
                }
                if (found){
                    found = false;
                    continue;
                }
                if (!((bool)(data_table[i][6]))){
                    time_result.Add(new Vehicle((string)(data_table[i][0]), i,Convert.ToDateTime(data_table[i][8])));
                }
                else{
                    time_result.Add(new Vehicle((string)(data_table[i][0]), true));
                }
            }
            uint temp_station_id = station_id;
            bool timed;
            bool final = false;
            foreach (Vehicle j in time_result){
                if (j.problem == true){
                    continue;
                }
                timed = false;
                int max = get_nr_stations(j.name) *4;
                int max_temp = max;
                while ((station_id != temp_station_id) ^ (!timed)){
                    if (max <= 0){
                        temp_station_id = station_id;
                        timed = false;
                        j.time = 0;
                        break;
                    }
                    timed = true;
                    final = false;
                    for (int i = j.index; i < data_table.Count; i++){
                        if ((string)(data_table[i][0]) != j.name){
                            break;
                        }
                        else if (final){
                            break;
                        }
                        else if ((temp_station_id == (uint)(data_table[i][3])) && ((int)data_table[i][7]==Get_tod())){
                            max = max_temp;
                            time_result.Find(x => x.name.Contains(j.name)).time += (uint)data_table[i][4];
                            temp_station_id = (uint)data_table[i][2];
                            for (int d = j.index; d < data_table.Count; d++){
                                if ((string)data_table[d][0] != j.name){
                                    break;
                                }
                                else if ((uint)data_table[d][2] == (uint)data_table[i][2] & (uint)data_table[d][1] == (uint)data_table[d][5]){
                                    final = true;
                                    temp_station_id = station_id;
                                    break;
                                }
                            }
                        }
                        else{
                            max--;
                        }
                    }
                }
            }
            foreach (Vehicle i in time_result)
            {
                i.Set_remaining_time();
            }
            ObservableCollection<Vehicle> rez = new ObservableCollection<Vehicle>(time_result);
            return rez;
        }
        public static ObservableCollection<List<object>> Get_stations(string vehicle_name) // Returns stations for a specific vehicle name.
        {
            ObservableCollection<List<object>> rez = new ObservableCollection<List<object>>();

            string data_string = "";
            Execute_querry($"stations[{vehicle_name}]", ref data_string);
            List<List<string>> data = new List<List<string>>(Parse_MSG(data_string));
            foreach (List<string> row in data)
            {
                if (row[0] != "")
                {
                    rez.Add(new List<object>(new object[] { Convert.ToInt32(row[0]), row[1], Convert.ToDecimal(row[2]), Convert.ToDecimal(row[3]) }));
                }
            }

            return rez;
        }
        public static ObservableCollection<List<object>> Get_Vehicle_location(string vehicle_name) // Return all coordinates for a specific vehicle name.
        {
            ObservableCollection<List<object>> vehicles = new ObservableCollection<List<object>>();
            string data_string = "";
            Execute_querry($"specific_vehicle[{vehicle_name}]",ref data_string);
            List<List<string>> data = new List<List<string>>(Parse_MSG(data_string));
            foreach (List<string> row in data)
            {
                if (row[0] != "")
                {
                    vehicles.Add(new List<object>(new object[] { Convert.ToDecimal(row[0]), Convert.ToDecimal(row[1]), Convert.ToBoolean(Convert.ToInt32(row[2])) }));
                }
            }
            return vehicles;
        }
        public static ObservableCollection<List<string>> Get_Search_results(string station_name) {
            if(station_name.Length > 2) {
                ObservableCollection<List<string>> vehicles = new ObservableCollection<List<string>>();
                string data_string = "";
                Execute_querry($"get_vehicles_from_station[{station_name}]", ref data_string);
                List<List<string>> data = new List<List<string>>(Parse_MSG(data_string));
                if(data[0][0] != "") {
                    foreach (List<string> row in data)
                    {
                        vehicles.Add(new List<string>(row));
                    }
                    return vehicles;
                }             
                return null;
            }
            return null;
        } // Returns all the vehicles that goes in this station. Search results are managed by SQL
        public static ObservableCollection<List<string>> Get_Vehicle_list() // Returns all unique vehicle names.
        {
            ObservableCollection<List<string>> vehicles = new ObservableCollection<List<string>>();
            string data_string = "";
            Execute_querry("view_vehicles_client[]", ref data_string);
            List<List<string>> data = new List<List<string>>(Parse_MSG(data_string));
            foreach (List<string> row in data)
            {
                vehicles.Add(new List<string>(row));
            }
            return vehicles;
        }
        public static ObservableCollection<List<string>> Get_Type_list()
        {
            ObservableCollection<List<string>> res = new ObservableCollection<List<string>>();
            string data_string = "";
            Execute_querry("get_types[]", ref data_string);
            List<List<string>> data = new List<List<string>>(Parse_MSG(data_string));
            foreach(List<string> row in data)
            {
                res.Add(new List<string>(row));
            }
            return res;
        } // Returns all types
        public static void Get_all_stations() // Returns all stations from DB
        {
            string data_string = "";
            Execute_querry("all_stations[]", ref data_string);
            List<List<string>> data = Parse_MSG(data_string);
            ObservableCollection<List<string>> temp = new ObservableCollection<List<string>>();
            foreach (List<string> row in data)
            {
                temp.Add(row);
            }
            Globals.all_stations = temp;
        }
        public static void Realtime_vehicle_location(){
            while (true) {
                if (Globals.vehicle_update) {
                    int? test = Globals.Stations?.Count;
                    if (test != null)
                    {
                        ObservableCollection<List<object>> listLoc = SQLInterface.Get_Vehicle_location(Globals.vname);

                        List<Pin> old = new List<Pin>();
                        foreach (Pin p in Globals.Maps.Pins) {
                            if (!Is_station(p.Position.Latitude, p.Position.Longitude)) {
                                old.Add(p);
                            }
                        }
                        foreach (List<object> row in listLoc) {
                            PageMap.PinsV(Globals.vname, Convert.ToDouble(row[0]), Convert.ToDouble(row[1]), Convert.ToBoolean(row[2]));
                        }

                        int length = Globals.Maps.Pins.Count;
                        for (int i = 0; i < length; i++) {
                            foreach (Pin o in old) {
                                if (Globals.Maps.Pins[i].Position.Latitude == o.Position.Latitude && Globals.Maps.Pins[i].Position.Longitude == o.Position.Longitude) {
                                    Globals.Maps.Pins.Remove(Globals.Maps.Pins[i]);
                                    old.Remove(o);
                                    break;
                                }
                            }
                        }
                    }
                }
            }
        }

        public static void Time_updater(){ // Gets the actual time of the server
            while (true)
            {
                if (Globals.updateServerTime)
                {
                    string data_string = "";
                    Execute_querry("get_current_time[]", ref data_string);
                    Globals.serverTime = Convert.ToDateTime(Parse_MSG(data_string)[0][0]);
                }
                Thread.Sleep(8000);

            }

        }
    }  
}
