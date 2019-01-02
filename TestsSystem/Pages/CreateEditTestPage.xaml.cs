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
            if (!SameTest(testNameTBox.Text) && !TestsService.isEditingTest) // Проверяем также какая кнопка была нажата (Редактировать тест или Создать)
            {
                if (FieldsFilled())
                {
                    CreateTest();
                    MainClass.FrameVar.Navigate(new AddQuestionsPage()); // Переходим на страницу добавления вопросов к этому тесту
                }
                else
                    MessageService.ShowError("Заполните все поля");
            }
            else if (TestsService.isEditingTest)
            {
                // Проверяем все ли поля заполнены
                if (FieldsFilled())
                {
                    UpdateTest();
                    TestsService.isEditingTest = false; // Говорим что редактирование кончилось
                    MessageService.ShowInfo("Информация о тесте успешно обновлена!");
                    MainClass.FrameVar.Navigate(new TeachersMenuPage()); // Возвращаемся в меню
                }
                else
                    MessageService.ShowError("Заполните все поля");
            }
            else
                MessageService.ShowError("Тест с таким именем уже существует!");
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            if (TestsService.isEditingTest)
                LoadPageForEdit(); // Если редактируем тест то загружаем изначальные данные о нем
            else 
                LoadPageForCreate(); // Иначе меняем названия кнопок и полей ввода для создания
        }

        void LoadPageForEdit()
        {

            testNameTBox.Text = TestsService.editingTest.Test_name;

            passHoursTBox.Value = CalcService.CalcHours(TestsService.editingTest.Pass_time); // Часы теста
            passMinutesTBox.Value = CalcService.CalcMinutes(TestsService.editingTest.Pass_time); // Минуты теста
            passSecondsTBox.Value = CalcService.CalcSeconds(TestsService.editingTest.Pass_time); // Секунды теста

            questionHoursTBox.Value =   CalcService.CalcHours(TestsService.editingTest.Question_time); // Часы вопроса
            questionMinutesTBox.Value = CalcService.CalcMinutes(TestsService.editingTest.Question_time); // Минуты вопроса
            questionSecondsTBox.Value = CalcService.CalcSeconds(TestsService.editingTest.Question_time); // Секунды вопроса

            passFiveTBox.Value =  TestsService.editingTest.Num_to_pass_five; // Баллов на 5
            passFourTBox.Value =  TestsService.editingTest.Num_to_pass_four; // Баллов на 4
            passThreeTBox.Value = TestsService.editingTest.Num_to_pass_three; // Баллов на 3

            createTestBut.Content = "Изменить информацию о тесте";
            this.Title = "Изменение информации о тесте";
        }

        void LoadPageForCreate()
        {
            this.Title = "Создание теста";
            testNameTBox.Text = "";
            passHoursTBox.Value = passMinutesTBox.Value = passSecondsTBox.Value = questionHoursTBox.Value = questionMinutesTBox.Value
              = questionSecondsTBox.Value = passFiveTBox.Value = passFourTBox.Value = passThreeTBox.Value = 0;
            createTestBut.Content = "Добавить вопросы для теста";
        }

        void UpdateTest()
        {
            // Меняем данные на пользовательские
            TestsService.editingTest.Author_id = UsersService.currentUser.id;
            TestsService.editingTest.Test_name = testNameTBox.Text;

            TestsService.editingTest.Pass_time =   CalcService.HoursToSeconds((int)passHoursTBox.Value)
                                                 + CalcService.MinutesToSeconds((int)passMinutesTBox.Value)
                                                 + Convert.ToInt32(passSecondsTBox.Value);

            TestsService.editingTest.Question_time =   CalcService.HoursToSeconds((int)questionHoursTBox.Value)
                                                     + CalcService.MinutesToSeconds((int)questionMinutesTBox.Value)
                                                     + Convert.ToInt32(questionSecondsTBox.Value);

            TestsService.editingTest.Num_to_pass_five =  Convert.ToInt32(passFiveTBox.Value);
            TestsService.editingTest.Num_to_pass_four =  Convert.ToInt32(passFourTBox.Value);
            TestsService.editingTest.Num_to_pass_three = Convert.ToInt32(passThreeTBox.Value);

            MainClass.db.SaveChanges();
        }

        bool SameTest(string testName)
        {
            var sameTestName = MainClass.db.Tests.Where(x => x.Test_name == testName).FirstOrDefault();

            if (sameTestName == null)
                return false;

            return true;
        }

        void CreateTest()
        {
            // Добавляем тест в БД
            Tests test = new Tests()
            {
                Author_id = UsersService.currentUser.id,
                Test_name = testNameTBox.Text,


                Pass_time =   CalcService.HoursToSeconds((int)passHoursTBox.Value)
                            + CalcService.MinutesToSeconds((int)passMinutesTBox.Value)
                            + Convert.ToInt32(passSecondsTBox.Value),


                Question_time =   CalcService.HoursToSeconds((int)questionHoursTBox.Value)
                                + CalcService.MinutesToSeconds((int)questionMinutesTBox.Value)
                                + Convert.ToInt32(questionSecondsTBox.Value),


                Num_to_pass_five =  Convert.ToInt32(passFiveTBox.Value),
                Num_to_pass_four =  Convert.ToInt32(passFourTBox.Value),
                Num_to_pass_three = Convert.ToInt32(passThreeTBox.Value)
            };
            MainClass.db.Tests.Add(test);
            MainClass.db.SaveChanges();

            TestsService.addedTest = test;
        }

        bool FieldsFilled()
        {
            if (testNameTBox.Text != null &&
                    (passHoursTBox.Value != 0 || passMinutesTBox.Value != 0 || passSecondsTBox.Value != 0) &&
                    (questionHoursTBox.Value != 0 || questionMinutesTBox.Value != 0 || questionSecondsTBox.Value != 0)
                    )
                return true;
            else
                return false;
        }
    }
}
