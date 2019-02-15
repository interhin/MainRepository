using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestsSystem
{
    public static class CalcService
    {
        public static int HoursToSeconds(int hours)
        {
            return hours * 3600;
        }

        public static int MinutesToSeconds(int minutes)
        {
            return minutes * 60;
        }

        public static int HoursToSeconds(string hours)
        {
            return int.Parse(hours) * 3600;
        }

        public static int MinutesToSeconds(string minutes)
        {
            return int.Parse(minutes) * 60;
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
