using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace model_metro
{
    internal class Passenger
    {
        public int StartStation;
        public int EndStation;
        public int TransferStation;
        public int ArrivalTime;
        public int TransferTime;
        public bool GreenLine;
        public int DepartureTime;
        public int Delay;
    }
}
