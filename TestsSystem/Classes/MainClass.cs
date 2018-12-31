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
        public static Tests_bdEntities1 db = new Tests_bdEntities1(); // Экземпляр БД

        public static string addedTestName { get; set; } // Имя добавленного теста
        public static int addedTestID { get; set; } // ID добавленного теста

        public static Frame FrameVar;

        public bool backButEn { get; set; } = false; // Switcher кнопки назад
        public static bool editingTest { get; set; } = false; // Проходит ли редактирование теста
        public static int editingTestID { get; set; } // ID Теста который редактируется

        public static int testingTestID { get; set; } // ID Теста который проходится в данный момент
        public static string testingTestName { get; set; } // Имя Теста который проходится в данный момент

        public MainClass()
        {
            backButEn = FrameVar.CanGoBack;
        }
    }
}
