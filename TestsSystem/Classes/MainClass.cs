using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using TestsSystem.Pages;
using TestsSystem.Models;

namespace TestsSystem
{
    public class MainClass
    {
        public static Tests_bdEntities db = new Tests_bdEntities(); // Экземпляр БД

        public static string addedTestName { get; set; } // Имя добавленного теста
        public static int addedTestID { get; set; } // ID добавленного теста

        public static Frame FrameVar;

        public static bool editingTest { get; set; } = false; // Проходит ли редактирование теста
        public static int editingTestID { get; set; } // ID Теста который редактируется

        public static int testingTestID { get; set; } // ID Теста который проходится в данный момент
        public static string testingTestName { get; set; } // Имя Теста который проходится в данный момент

        public static bool disableBack { get; set; } = false;

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
