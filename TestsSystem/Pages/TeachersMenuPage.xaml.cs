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
            // Говорим что мы не редактируем а создаем тест и открываем страницу создания
            MainClass.editingTest = false;
            MainClass.FrameVar.Navigate(new CreateEditTestPage());
        }

        private void editTestBut_Click(object sender, RoutedEventArgs e)
        {
            // Говорим что мы редактируем тест, запоминаем его ID и открываем страницу редактирования
            MainClass.editingTest = true;
            MainClass.editingTestID = Convert.ToInt32(testsCB.SelectedValue);
            MainClass.FrameVar.Navigate(new CreateEditTestPage());
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            // Загружаем список тестов пользователя и отключаем кнопки
            LoadTests();
            TurnOffButs();
        }

        private void testsCB_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            TurnOnButs();
        }

        private void editTestsQuestionsBut_Click(object sender, RoutedEventArgs e)
        {
            // Запоминаем ID теста и открываем страницу редактирования вопросов
            MainClass.editingTestID = Convert.ToInt32(testsCB.SelectedValue);
            MainClass.FrameVar.Navigate(new EditTestsQuestionsPage());
        }

        void TurnOffButs()
        {
            editTestBut.IsEnabled = false;
            editTestsQuestionsBut.IsEnabled = false;
        }
        
        void TurnOnButs()
        {
            editTestBut.IsEnabled = true;
            editTestsQuestionsBut.IsEnabled = true;
        }

        void LoadTests()
        {
            testsCB.ItemsSource = MainClass.db.Tests.Where(x => x.Author_id == CurrentUser.curUser.id).ToList();
            testsCB.DisplayMemberPath = "Test_name";
            testsCB.SelectedValuePath = "id";
            testsCB.SelectedIndex = -1;
        }
    }
}
