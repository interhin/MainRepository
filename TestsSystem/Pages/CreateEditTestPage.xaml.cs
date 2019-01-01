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
    /// Interaction logic for CreateTest.xaml
    /// </summary>
    public partial class CreateEditTestPage : Page
    {
        public CreateEditTestPage()
        {
            InitializeComponent();
        }

        private void createTestBut_Click(object sender, RoutedEventArgs e)
        {
            // Проверяем есть ли уже тест с таким именем
            var sameTestName = MainClass.db.Tests.Where(x => x.Test_name == testNameTBox.Text).FirstOrDefault();
            if (sameTestName == null && !MainClass.editingTest) // Проверяем также какая кнопка была нажата (Редактировать тест или Создать)
            {
                createTest();
            }
            else if (MainClass.editingTest)
            {
                updateTest();
            }
            else
                MessageBox.Show("Тест с таким именем уже существует", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            if (MainClass.editingTest)
            {
                // Если редактируем тест то загружаем изначальные данные о нем
                var test = MainClass.db.Tests.Where(x => x.id == MainClass.editingTestID).First();
                testNameTBox.Text = test.Test_name;
                passHoursTBox.Text = (test.Pass_time / 3600).ToString();
                passMinutesTBox.Text = ((test.Pass_time % 3600) / 60).ToString();
                passSecondsTBox.Text = ((test.Pass_time % 3600) % 60).ToString();
                questionHoursTBox.Text = (test.Question_time / 3600).ToString();
                questionMinutesTBox.Text = ((test.Question_time % 3600) / 60).ToString();
                questionSecondsTBox.Text = ((test.Question_time % 3600) % 60).ToString();
                passFiveTBox.Text = test.Num_to_pass_five.ToString();
                passFourTBox.Text = test.Num_to_pass_four.ToString();
                passThreeTBox.Text = test.Num_to_pass_three.ToString();
                createTestBut.Content = "Изменить информацию о тесте";
                this.Title = "Изменение информации о тесте";
            }
            else
            {
                // Иначе меняем названия кнопок и полей ввода для создания
                this.Title = "Создание теста";
                testNameTBox.Text = "";
                passHoursTBox.Text = passMinutesTBox.Text = passSecondsTBox.Text = questionHoursTBox.Text = questionMinutesTBox.Text
                  = questionSecondsTBox.Text = passFiveTBox.Text = passFourTBox.Text = passThreeTBox.Text = "0";
                createTestBut.Content = "Добавить вопросы для теста";
            }
        }

        void updateTest()
        {
            // Проверяем все ли поля заполнены
            if (testNameTBox.Text != null &&
                    (passHoursTBox.Text != "0" || passMinutesTBox.Text != "0" || passSecondsTBox.Text != "0") &&
                    (questionHoursTBox.Text != "0" || questionMinutesTBox.Text != "0" || questionSecondsTBox.Text != "0") &&
                    (passFiveTBox.Text != "0" && passFourTBox.Text != "0" && passThreeTBox.Text != "0")
                    )
            {
                // Находим в БД тест который мы будем обновлять
                var updTest = MainClass.db.Tests.Where(x => x.id == MainClass.editingTestID).First();
                // Меняем данные на пользовательские
                updTest.Author_id = CurrentUser.curUser.id;
                updTest.Test_name = testNameTBox.Text;
                updTest.Pass_time = Convert.ToInt32(passHoursTBox.Text) * 3600 // Часы теста
                                    + Convert.ToInt32(passMinutesTBox.Text) * 60 // Минуты теста
                                    + Convert.ToInt32(passSecondsTBox.Text); // Секунды теста
                updTest.Question_time = Convert.ToInt32(questionHoursTBox.Text) * 3600 // Часы вопроса
                                    + Convert.ToInt32(questionMinutesTBox.Text) * 60  // Минуты вопроса
                                    + Convert.ToInt32(questionSecondsTBox.Text); // Секунды вопроса
                updTest.Num_to_pass_five = Convert.ToInt32(passFiveTBox.Text);
                updTest.Num_to_pass_four = Convert.ToInt32(passFourTBox.Text);
                updTest.Num_to_pass_three = Convert.ToInt32(passThreeTBox.Text);
                MainClass.db.SaveChanges();
                MainClass.editingTest = false; // Говорим что редактирование кончилось
                MessageBox.Show("Информация о тесте успешно обновлена!", "Информация", MessageBoxButton.OK, MessageBoxImage.Information);
                MainClass.FrameVar.Navigate(new TeachersMenuPage()); // Возвращаемся в меню
            }
            else
                MessageBox.Show("Заполните все поля", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
        }

        void createTest()
        {
            if (testNameTBox.Text != null &&
                    (passHoursTBox.Text != "0" || passMinutesTBox.Text != "0" || passSecondsTBox.Text != "0") &&
                    (questionHoursTBox.Text != "0" || questionMinutesTBox.Text != "0" || questionSecondsTBox.Text != "0") &&
                    (passFiveTBox.Text != "0" && passFourTBox.Text != "0" && passThreeTBox.Text != "0")
                    )
            {
                // Добавляем тест в БД
                Tests test = new Tests()
                {
                    Author_id = CurrentUser.curUser.id,
                    Test_name = testNameTBox.Text,
                    Pass_time = Convert.ToInt32(passHoursTBox.Text) * 3600
                                + Convert.ToInt32(passMinutesTBox.Text) * 60
                                + Convert.ToInt32(passSecondsTBox.Text),
                    Question_time = Convert.ToInt32(questionHoursTBox.Text) * 3600
                                + Convert.ToInt32(questionMinutesTBox.Text) * 60
                                + Convert.ToInt32(questionSecondsTBox.Text),
                    Num_to_pass_five = Convert.ToInt32(passFiveTBox.Text),
                    Num_to_pass_four = Convert.ToInt32(passFourTBox.Text),
                    Num_to_pass_three = Convert.ToInt32(passThreeTBox.Text)
                };
                MainClass.db.Tests.Add(test);
                MainClass.db.SaveChanges();
                var addedTestVar = MainClass.db.Tests.Where(x => x.Test_name == testNameTBox.Text).First();
                MainClass.addedTestID = addedTestVar.id;
                MainClass.addedTestName = testNameTBox.Text;
                MainClass.FrameVar.Navigate(new AddQuestionsPage()); // Переходим на страницу добавления вопросов к этому теса
            }
            else
                MessageBox.Show("Заполните все поля", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }
}
