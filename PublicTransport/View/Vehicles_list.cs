using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace PublicTransport.View
{
    class Vehicles_list
    {
        public string Name { get; set; }

        Vehicles_list() { }

        public string Get_name_veh()
        {
            return Name;
        }
        public Vehicles_list(string Name)
        {
            this.Name = Name;
        }

        public static ObservableCollection<Vehicles_list> Get_vehicles_list(string type)
        {
            ObservableCollection<Vehicles_list> res = new ObservableCollection<Vehicles_list>();
            foreach(List<string> row in Globals.vehicles_list)
            {
                if(type == row[1])
                {
                    res.Add(new Vehicles_list(row[0]));
                }
            }
            return res;
        }
    }
}
