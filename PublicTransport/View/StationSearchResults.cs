using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace PublicTransport.View
{
    class StationSearchResults
    {
        public string Name { get; set; }

        public StationSearchResults() { }

        public StationSearchResults(string Nume)
        {
            Name = Nume;
        }

        public string Get_nume_Station()
        {
            return Name;
        }

        public static ObservableCollection<StationSearchResults> Get_results(ObservableCollection<List<string>> x)
        {
            if (x != null)
            {
                ObservableCollection<StationSearchResults> res = new ObservableCollection<StationSearchResults>();
                foreach (List<string> row in x)
                {
                    res.Add(new StationSearchResults(row[0]));
                }
                return res;
            }
            else return null;
            
        }


    }
}
