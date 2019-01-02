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

            passHoursTBox.Text =   CalcService.CalcHours(TestsService.editingTest.Pass_time).ToString(); // Часы теста
            passMinutesTBox.Text = CalcService.CalcMinutes(TestsService.editingTest.Pass_time).ToString(); // Минуты теста
            passSecondsTBox.Text = CalcService.CalcSeconds(TestsService.editingTest.Pass_time).ToString(); // Секунды теста

            questionHoursTBox.Text =   CalcService.CalcHours(TestsService.editingTest.Question_time).ToString(); // Часы вопроса
            questionMinutesTBox.Text = CalcService.CalcMinutes(TestsService.editingTest.Question_time).ToString(); // Минуты вопроса
            questionSecondsTBox.Text = CalcService.CalcSeconds(TestsService.editingTest.Question_time).ToString(); // Секунды вопроса

            passFiveTBox.Text =  TestsService.editingTest.Num_to_pass_five.ToString(); // Баллов на 5
            passFourTBox.Text =  TestsService.editingTest.Num_to_pass_four.ToString(); // Баллов на 4
            passThreeTBox.Text = TestsService.editingTest.Num_to_pass_three.ToString(); // Баллов на 3

            createTestBut.Content = "Изменить информацию о тесте";
            this.Title = "Изменение информации о тесте";
        }

        void LoadPageForCreate()
        {
            this.Title = "Создание теста";
            testNameTBox.Text = "";
            passHoursTBox.Text = passMinutesTBox.Text = passSecondsTBox.Text = questionHoursTBox.Text = questionMinutesTBox.Text
              = questionSecondsTBox.Text = passFiveTBox.Text = passFourTBox.Text = passThreeTBox.Text = "0";
            createTestBut.Content = "Добавить вопросы для теста";
        }

        void UpdateTest()
        {
            // Меняем данные на пользовательские
            TestsService.editingTest.Author_id = UsersService.currentUser.id;
            TestsService.editingTest.Test_name = testNameTBox.Text;

            TestsService.editingTest.Pass_time =   CalcService.HoursToSeconds(passHoursTBox.Text)
                                                 + CalcService.MinutesToSeconds(passMinutesTBox.Text)
                                                 + Convert.ToInt32(passSecondsTBox.Text);

            TestsService.editingTest.Question_time =   CalcService.HoursToSeconds(questionHoursTBox.Text)
                                                     + CalcService.MinutesToSeconds(questionMinutesTBox.Text)
                                                     + Convert.ToInt32(questionSecondsTBox.Text);

            TestsService.editingTest.Num_to_pass_five =  Convert.ToInt32(passFiveTBox.Text);
            TestsService.editingTest.Num_to_pass_four =  Convert.ToInt32(passFourTBox.Text);
            TestsService.editingTest.Num_to_pass_three = Convert.ToInt32(passThreeTBox.Text);

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


                Pass_time =   CalcService.HoursToSeconds(passHoursTBox.Text)
                            + CalcService.MinutesToSeconds(passMinutesTBox.Text)
                            + Convert.ToInt32(passSecondsTBox.Text),


                Question_time =   CalcService.HoursToSeconds(questionHoursTBox.Text)
                                + CalcService.MinutesToSeconds(questionMinutesTBox.Text)
                                + Convert.ToInt32(questionSecondsTBox.Text),


                Num_to_pass_five =  Convert.ToInt32(passFiveTBox.Text),
                Num_to_pass_four =  Convert.ToInt32(passFourTBox.Text),
                Num_to_pass_three = Convert.ToInt32(passThreeTBox.Text)
            };
            MainClass.db.Tests.Add(test);
            MainClass.db.SaveChanges();

            TestsService.addedTest = test;
        }

        bool FieldsFilled()
        {
            if (testNameTBox.Text != null &&
                    (passHoursTBox.Text != "0" || passMinutesTBox.Text != "0" || passSecondsTBox.Text != "0") &&
                    (questionHoursTBox.Text != "0" || questionMinutesTBox.Text != "0" || questionSecondsTBox.Text != "0")
                    )
                return true;
            else
                return false;
        }
    }
}
