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

        private void startTestBut_Click(object sender, RoutedEventArgs e)
        {
            // Если тест не выбран то ничего не делаем
            if (testsLB.SelectedItem == null)
                return;

            // Если выбран то сохраняем данные о тесте и переходим на другую страницу
            MainClass.studentTestingID = Convert.ToInt32(testsLB.SelectedValue);
            MainClass.studentTestingName = (testsLB.SelectedItem as Tests).Test_name;
            MainClass.FrameVar.Navigate(new testingPage());
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {

            List<int> passedTestsList = new List<int>();
            // Находим все тесты которые уже прошел пользователь
            passedTestsList = MainClass.db.History.Where(x => x.User_id == CurrentUser.curUser.id).Select(x => x.Test_id).ToList();
            testsLB.DisplayMemberPath = "Test_name";
            testsLB.SelectedValuePath = "id";
            // Достаем все тесты из БД исключая тесты из списка passedTestsList
            testsLB.ItemsSource = MainClass.db.Tests.Where(x=>!passedTestsList.Contains(x.id)).ToList();
            // Загружаем пройденные тесты в DataGrid
            historyDG.ItemsSource = MainClass.db.History.Where(x => x.User_id == CurrentUser.curUser.id).ToList();
        }
    }
}
