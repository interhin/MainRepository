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
    /// Interaction logic for MenuPanel.xaml
    /// </summary>
    public partial class TeachersMenuPage : Page
    {
        public TeachersMenuPage()
        {
            InitializeComponent();
        }

        private void createTestBut_Click(object sender, RoutedEventArgs e)
        {
            MainClass.editingTest = false;
            MainClass.FrameVar.Navigate(new CreateTestPage());
        }

        private void editTestBut_Click(object sender, RoutedEventArgs e)
        {
            MainClass.editingTest = true;
            MainClass.editingTestID = Convert.ToInt32(testsGB.SelectedValue);
            MainClass.FrameVar.Navigate(new CreateTestPage());
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            testsGB.ItemsSource = MainClass.db.Tests.Where(x => x.Author_id == CurrentUser.curUser.id).ToList();
            testsGB.DisplayMemberPath = "Test_name";
            testsGB.SelectedValuePath = "id";
            testsGB.SelectedIndex = -1;
            editTestBut.IsEnabled = false;
            editTestsQuestions.IsEnabled = false;
        }

        private void testsGB_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            editTestBut.IsEnabled = true;
            editTestsQuestions.IsEnabled = true;
        }

        private void editTestsQuestions_Click(object sender, RoutedEventArgs e)
        {
            MainClass.editingTestID = Convert.ToInt32(testsGB.SelectedValue);
            MainClass.FrameVar.Navigate(new EditTestsQuestionsPage());
        }
    }
}
