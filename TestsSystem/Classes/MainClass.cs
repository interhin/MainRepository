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
        public static Tests_bdEntities1 db = new Tests_bdEntities1();

        public static string addedTestName { get; set; }
        public static int addedTestId { get; set; }

        public static Frame FrameVar;

        public bool backButEn { get; set; } = false;
        public static bool editingTest { get; set; } = false;
        public static int editingTestId { get; set; }

        public static int studentTestingID { get; set; }
        public static string studentTestingName { get; set; }
        public MainClass()
        {
            backButEn = FrameVar.CanGoBack;
        }
    }
}
