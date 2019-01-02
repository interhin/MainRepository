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
            TestsService.isEditingTest = false;
            MainClass.FrameVar.Navigate(new CreateEditTestPage());
        }

        private void editTestBut_Click(object sender, RoutedEventArgs e)
        {
            // Говорим что мы редактируем тест, запоминаем его ID и открываем страницу редактирования
            TestsService.isEditingTest = true;
            TestsService.editingTest = testsCB.SelectedItem as Tests;
            MainClass.FrameVar.Navigate(new CreateEditTestPage());
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            // Загружаем список тестов пользователя и отключаем кнопки
            LoadTests();
            DisableButs();
        }

        private void testsCB_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            EnableButs();
        }

        private void editTestsQuestionsBut_Click(object sender, RoutedEventArgs e)
        {
            // Запоминаем ID теста и открываем страницу редактирования вопросов
            TestsService.editingTest = testsCB.SelectedItem as Tests;
            MainClass.FrameVar.Navigate(new EditTestsQuestionsPage());
        }

        void DisableButs()
        {
            editTestBut.IsEnabled = false;
            editTestsQuestionsBut.IsEnabled = false;
        }
        
        void EnableButs()
        {
            editTestBut.IsEnabled = true;
            editTestsQuestionsBut.IsEnabled = true;
        }

        void LoadTests()
        {
            testsCB.ItemsSource = MainClass.db.Tests.Where(x => x.Author_id == UsersService.currentUser.id).ToList();
            testsCB.DisplayMemberPath = "Test_name";
            testsCB.SelectedValuePath = "id";
            testsCB.SelectedIndex = -1;
        }
    }
}
