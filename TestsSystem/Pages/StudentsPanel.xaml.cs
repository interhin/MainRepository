using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using TestsSystem.Models;

namespace TestsSystem.Pages
{
    /// <summary>
    /// Interaction logic for StudentsPanel.xaml
    /// </summary>
    public partial class StudentsPanel : Page
    {
        public StudentsPanel()
        {
            InitializeComponent();
        }

        private void startTest_Click(object sender, RoutedEventArgs e)
        {
            if (testsList.SelectedItem == null)
                return;
            MainClass.studentTestingID = Convert.ToInt32(testsList.SelectedValue);
            MainClass.studentTestingName = (testsList.SelectedItem as Tests).Test_name;
            MainClass.FrameVar.Navigate(new testingPage());
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            List<int> passedTests = new List<int>();
            passedTests = MainClass.db.History.Where(x => x.User_id == CurrentUser.curUser.id).Select(x => x.Test_id).ToList();
            testsList.DisplayMemberPath = "Test_name";
            testsList.SelectedValuePath = "id";
            testsList.ItemsSource = MainClass.db.Tests.Where(x=>!passedTests.Contains(x.id)).ToList();
            historyGrid.ItemsSource = MainClass.db.History.Where(x => x.User_id == CurrentUser.curUser.id).ToList();
        }
    }
}
