using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using TestsSystem.Pages;
using TestsSystem.Models;
using System.Windows;

namespace TestsSystem
{
    public static class MainClass
    {
        public static TestsDBEntities db = new TestsDBEntities(); // Экземпляр БД

        public static Frame FrameVar;

        public static bool disableBack { get; set; } = false;

        public static void DisableGlowAllItems(ref ListBox listbox)
        {
            foreach (var item in listbox.Items)
            {
                (listbox.ItemContainerGenerator.ContainerFromItem(item) as ListBoxItem).Style = null;
            }
        }

        public static void GlowSelectedItem(ref ListBox listbox)
        {
            var correctStyle = Application.Current.FindResource("correctAnswerStyle") as Style;
            (listbox.ItemContainerGenerator.ContainerFromItem(listbox.SelectedItem) as ListBoxItem).Style = correctStyle;
        }
    }
}
