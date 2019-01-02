using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestsSystem
{
    public static class CalcService
    {
        public static int HoursToSeconds(int seconds)
        {
            return seconds * 3600;
        }

        public static int MinutesToSeconds(int seconds)
        {
            return seconds * 60;
        }

        public static int HoursToSeconds(string seconds)
        {
            return Convert.ToInt32(seconds) * 3600;
        }

        public static int MinutesToSeconds(string seconds)
        {
            return Convert.ToInt32(seconds) * 60;
        }

        public static int CalcHours(int seconds)
        {
            return seconds / 3600;
        }

        public static int CalcMinutes(int seconds)
        {
            return (seconds % 3600) / 60;
        }

        public static int CalcSeconds(int seconds)
        {
            return (seconds % 3600) % 60;
        }
    }
}
