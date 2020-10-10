using client_data_functions;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace PublicTransport.View
{
    public class Type
    {
        public string Name { get; set; }

        public Type() { }

        public Type(string Nume)
        {
            Name = Nume;
        }

        public string Get_nume_type()
        {
            return Name;
        }
        public static ObservableCollection<Type> Get_types()
        {
            ObservableCollection<Type> res = new ObservableCollection<Type>();
            ObservableCollection<List<string>> data = SQLInterface.Get_Type_list();
            foreach (List<string> row in data)
            {
                res.Add(new Type(row[0]));
            }
            return res;
        }
    }
}
