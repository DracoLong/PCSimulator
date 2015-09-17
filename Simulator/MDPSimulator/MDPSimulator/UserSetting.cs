using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MDPSimulator
{
    public static class UserSetting
    {
        public static int TimeLimit { get; set; }
        public static int CoverageLimit { get; set; }
        public static int Speed { get; set;}
        static UserSetting()
        {
            TimeLimit = 360;
            CoverageLimit = 100;
            Speed = 3;
        }
    }
}
