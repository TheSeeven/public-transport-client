namespace PublicTransport
{
    public class VehicleTime
    {
        public string name;
        public uint time;
        public bool problem;
        public int index;

        public VehicleTime(string Name,int Index) {
            name = Name;
            time = 0;
            problem = false;
            index = Index;
        }
        public VehicleTime(string Name,bool Problem)
        {
            name = Name;
            time = 0;
            problem = Problem;
        }
        public VehicleTime() { }

    }
}
