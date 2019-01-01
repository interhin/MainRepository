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
    public partial class StudentsMenuPage : Page
    {
        public StudentsMenuPage()
        {
            InitializeComponent();
        }

        private void startTestBut_Click(object sender, RoutedEventArgs e)
        {
            // Если выбран то сохраняем данные о тесте и переходим к тестированию
            MainClass.testingTestID = Convert.ToInt32(testsLB.SelectedValue);
            MainClass.testingTestName = (testsLB.SelectedItem as Tests).Test_name;
            MainClass.FrameVar.Navigate(new TestingPage());
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            LoadNotPassedTests();
            LoadUserHistory();
        }


        void LoadNotPassedTests()
        {
            List<int> passedTestsList = new List<int>();
            // Находим все тесты которые уже прошел пользователь
            passedTestsList = LoadPassedTests(CurrentUser.curUser.id);
            testsLB.DisplayMemberPath = "Test_name";
            testsLB.SelectedValuePath = "id";
            // Достаем все тесты из БД исключая тесты из списка passedTestsList
            testsLB.ItemsSource = MainClass.db.Tests.Where(x => !passedTestsList.Contains(x.id)).ToList();
        }

        List<int> LoadPassedTests(int userID)
        {
            return MainClass.db.History.Where(x => x.User_id == userID).Select(x => x.Test_id).ToList();
        }

        void LoadUserHistory()
        {
            // Загружаем пройденные тесты в DataGrid
            historyDG.ItemsSource = MainClass.db.History.Where(x => x.User_id == CurrentUser.curUser.id).ToList();
        }

        private void testsLB_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            startTestBut.IsEnabled = true;
        }
    }
}
