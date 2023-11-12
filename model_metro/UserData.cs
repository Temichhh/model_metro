using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlTypes;

namespace model_metro
{
    public class UserData
    {
        public int StartHour { get; set; }
        public int StartMinute { get; set; }
        public int EndHour { get; set; }
        public int EndMinute { get; set; }
        public int RedBranchValue { get; set; }
        public int GreenBranchValue { get; set; }
        public UserData() 
        { 
            StartHour = 6;
            StartMinute = 0;
            EndHour = 7;
            EndMinute = 15;
            RedBranchValue = 5;
            GreenBranchValue = 10;
        }
        public void SetUserData(int st_h, int st_min, int end_h, int end_min, int red, int green)
        {
            StartHour= st_h;
            StartMinute= st_min;
            EndHour= end_h;
            EndMinute= end_min;
            RedBranchValue = red;
            GreenBranchValue = green;
        }
       
    }
}
