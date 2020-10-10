using System;
using PublicTransport;

namespace client_data_functions
{
    public class Vehicle
    {
        public string name;
        public uint time;
        public bool problem;
        public int index;
        public DateTime arrival;

        public Vehicle(string Name,int Index,DateTime Arrival) {
            name = Name;
            time = 0;
            problem = false;
            index = Index;
            arrival = Arrival;
        }
        public Vehicle(string Name,bool Problem)
        {
            name = Name;
            time = 0;
            problem = Problem;
        }


        public Vehicle() { }

        public void Set_remaining_time()
        {
            try 
            {
                time = Convert.ToUInt32((arrival.AddMinutes(time)).Subtract(Globals.serverTime).TotalMinutes);
            }
            catch
            {
                time=0;
            }
            
            
        }
    }
}
