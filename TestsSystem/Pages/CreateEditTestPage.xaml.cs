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
                    MainClass.FrameVar.Navigate(new EditTestsQuestionsPage()); // Переходим на страницу добавления вопросов к этому тесту
                }
                else
                    MessageService.ShowError("Заполните все поля");
            }
            else if (TestsService.isEditingTest)
            {
                // Проверяем все ли поля заполнены
                if (FieldsFilled())
                {
                    if (!TestsService.isImportingTest)
                    {
                        UpdateTest();
                        TestsService.isEditingTest = false; // Говорим что редактирование кончилось
                        MessageService.ShowInfo("Информация о тесте успешно обновлена!");
                        MainClass.FrameVar.Navigate(new TeachersMenuPage()); // Возвращаемся в меню
                    }
                    else
                    {
                        UpdateTest();
                        MainClass.FrameVar.Navigate(new EditTestsQuestionsPage());
                    }
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

            LoadTestInfo();

            createTestBut.Content = "Изменить вопросы теста";
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

        void LoadPageForImportEdit()
        {
            LoadTestInfo();
            createTestBut.Content = "Редактировать вопросы теста";
        }

        void LoadTestInfo()
        {
            testNameTBox.Text = TestsService.editingTest.Name;

            passHoursTBox.Value = CalcService.CalcHours(TestsService.editingTest.PassTime); // Часы теста
            passMinutesTBox.Value = CalcService.CalcMinutes(TestsService.editingTest.PassTime); // Минуты теста
            passSecondsTBox.Value = CalcService.CalcSeconds(TestsService.editingTest.PassTime); // Секунды теста

            questionHoursTBox.Value = CalcService.CalcHours(TestsService.editingTest.QuestionTime); // Часы вопроса
            questionMinutesTBox.Value = CalcService.CalcMinutes(TestsService.editingTest.QuestionTime); // Минуты вопроса
            questionSecondsTBox.Value = CalcService.CalcSeconds(TestsService.editingTest.QuestionTime); // Секунды вопроса

            passFiveTBox.Value = TestsService.editingTest.NumToPassFive; // Баллов на 5
            passFourTBox.Value = TestsService.editingTest.NumToPassFour; // Баллов на 4
            passThreeTBox.Value = TestsService.editingTest.NumToPassThree; // Баллов на 3
        }

        void UpdateTest()
        {
            // Меняем данные на пользовательские
            TestsService.editingTest.AuthorID = UsersService.currentUser.id;
            TestsService.editingTest.Name = testNameTBox.Text;

            TestsService.editingTest.PassTime =   CalcService.HoursToSeconds((int)passHoursTBox.Value)
                                                 + CalcService.MinutesToSeconds((int)passMinutesTBox.Value)
                                                 + Convert.ToInt32(passSecondsTBox.Value);

            TestsService.editingTest.QuestionTime =   CalcService.HoursToSeconds((int)questionHoursTBox.Value)
                                                     + CalcService.MinutesToSeconds((int)questionMinutesTBox.Value)
                                                     + Convert.ToInt32(questionSecondsTBox.Value);

            TestsService.editingTest.NumToPassFive =  Convert.ToInt32(passFiveTBox.Value);
            TestsService.editingTest.NumToPassFour =  Convert.ToInt32(passFourTBox.Value);
            TestsService.editingTest.NumToPassThree = Convert.ToInt32(passThreeTBox.Value);

            MainClass.db.SaveChanges();
        }

        bool SameTest(string testName)
        {
            var sameTestName = MainClass.db.Tests.Where(x => x.Name == testName).FirstOrDefault();

            if (sameTestName == null)
                return false;

            return true;
        }

        void CreateTest()
        {
            // Добавляем тест в БД
            Tests test = new Tests()
            {
                AuthorID = UsersService.currentUser.id,
                Name = testNameTBox.Text,


                PassTime =   CalcService.HoursToSeconds((int)passHoursTBox.Value)
                            + CalcService.MinutesToSeconds((int)passMinutesTBox.Value)
                            + Convert.ToInt32(passSecondsTBox.Value),


                QuestionTime =   CalcService.HoursToSeconds((int)questionHoursTBox.Value)
                                + CalcService.MinutesToSeconds((int)questionMinutesTBox.Value)
                                + Convert.ToInt32(questionSecondsTBox.Value),


                NumToPassFive =  Convert.ToInt32(passFiveTBox.Value),
                NumToPassFour =  Convert.ToInt32(passFourTBox.Value),
                NumToPassThree = Convert.ToInt32(passThreeTBox.Value)
            };
            MainClass.db.Tests.Add(test);
            MainClass.db.SaveChanges();

            TestsService.isEditingTest = true;
            TestsService.editingTest = test;
            //TestsService.addedTest = test;
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
