using client_data_functions;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace PublicTransport.View
{
    public class Categories
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public decimal Lat { get; set; }
        public decimal Lng { get; set; }
        public Categories() { }
        public Categories(int Id, string Name, decimal Lat, decimal Lng) {
            this.Id = Id;
            this.Name = Name;
            this.Lat = Lat;
            this.Lng = Lng;
        }
        public static ObservableCollection<Categories> Get_categories(string veh)
        {
            ObservableCollection<Categories> x = new ObservableCollection<Categories>();
            foreach(List<object> row in SQLInterface.Get_stations(veh))
            {
                x.Add(new Categories(Convert.ToInt32(row[0]), row[1].ToString(), Convert.ToDecimal(row[2]), Convert.ToDecimal(row[3])));
            }
            return x;
        }
    }
}
