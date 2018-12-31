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
    public partial class CreateTest : Page
    {
        public CreateTest()
        {
            InitializeComponent();
        }

        private void createTestBut_Click(object sender, RoutedEventArgs e)
        {
            var sameTestName = MainClass.db.Tests.Where(x => x.Test_name == testName.Text).FirstOrDefault();
            if (sameTestName == null && !MainClass.editingTest)
            {
                createTest();
            }
            else if (MainClass.editingTest)
            {
                updateTest();
            } else
                MessageBox.Show("Тест с таким именем уже существует", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            if (MainClass.editingTest)
            {
                var test = MainClass.db.Tests.Where(x => x.id == MainClass.editingTestID).First();
                testName.Text = test.Test_name;
                timeToPassHours.Text = (test.Pass_time / 3600).ToString();
                timeToPassMinutes.Text = ((test.Pass_time % 3600) / 60).ToString();
                timeToPassSeconds.Text = ((test.Pass_time % 3600) % 60).ToString();
                timeToQuestHours.Text = (test.Question_time / 3600).ToString();
                timeToQuestMinutes.Text = ((test.Question_time % 3600) / 60).ToString();
                timeToQuestSeconds.Text = ((test.Question_time % 3600) % 60).ToString();
                numToPassFive.Text = test.Num_to_pass_five.ToString();
                numToPassFour.Text = test.Num_to_pass_four.ToString();
                numToPassThree.Text = test.Num_to_pass_three.ToString();
                createTestBut.Content = "Изменить информацию о тесте";
                this.Title = "Изменение информации о тесте";
            }
            else
            {
                this.Title = "Создание теста";
                testName.Text = "";
                timeToPassHours.Text = timeToPassMinutes.Text = timeToPassSeconds.Text = timeToQuestHours.Text = timeToQuestMinutes.Text
                  = timeToQuestSeconds.Text = numToPassFive.Text = numToPassFour.Text = numToPassThree.Text = "0";
                createTestBut.Content = "Добавить вопросы для теста";
            }
        }

        void updateTest()
        {
            if (testName.Text != null &&
                    (timeToPassHours.Text != "0" || timeToPassMinutes.Text != "0" || timeToPassSeconds.Text != "0") &&
                    (timeToQuestHours.Text != "0" || timeToQuestMinutes.Text != "0" || timeToQuestSeconds.Text != "0") &&
                    (numToPassFive.Text != "0" && numToPassFour.Text != "0" && numToPassThree.Text != "0")
                    )
            {
                var updTest = MainClass.db.Tests.Where(x => x.id == MainClass.editingTestID).First();
                updTest.Author_id = CurrentUser.curUser.id;
                updTest.Test_name = testName.Text;
                updTest.Pass_time = Convert.ToInt32(timeToPassHours.Text) * 3600 + Convert.ToInt32(timeToPassMinutes.Text) * 60 + Convert.ToInt32(timeToPassSeconds.Text);
                updTest.Question_time = Convert.ToInt32(timeToQuestHours.Text) * 3600 + Convert.ToInt32(timeToQuestMinutes.Text) * 60 + Convert.ToInt32(timeToQuestSeconds.Text);
                updTest.Num_to_pass_five = Convert.ToInt32(numToPassFive.Text);
                updTest.Num_to_pass_four = Convert.ToInt32(numToPassFour.Text);
                updTest.Num_to_pass_three = Convert.ToInt32(numToPassThree.Text);
                MainClass.db.SaveChanges();
                MainClass.editingTest = false;
                MessageBox.Show("Информация о тесте успешно обновлена!","Информация",MessageBoxButton.OK,MessageBoxImage.Information);
                MainClass.FrameVar.Navigate(new MenuPanel());
            }
            else
                MessageBox.Show("Заполните все поля", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
        }

        void createTest()
        {
            if (testName.Text != null &&
                    (timeToPassHours.Text != "0" || timeToPassMinutes.Text != "0" || timeToPassSeconds.Text != "0") &&
                    (timeToQuestHours.Text != "0" || timeToQuestMinutes.Text != "0" || timeToQuestSeconds.Text != "0") &&
                    (numToPassFive.Text != "0" && numToPassFour.Text != "0" && numToPassThree.Text != "0")
                    )
            {
                Tests test = new Tests()
                {
                    Author_id = CurrentUser.curUser.id,
                    Test_name = testName.Text,
                    Pass_time = Convert.ToInt32(timeToPassHours.Text) * 3600 + Convert.ToInt32(timeToPassMinutes.Text) * 60 + Convert.ToInt32(timeToPassSeconds.Text),
                    Question_time = Convert.ToInt32(timeToQuestHours.Text) * 3600 + Convert.ToInt32(timeToQuestMinutes.Text) * 60 + Convert.ToInt32(timeToQuestSeconds.Text),
                    Num_to_pass_five = Convert.ToInt32(numToPassFive.Text),
                    Num_to_pass_four = Convert.ToInt32(numToPassFour.Text),
                    Num_to_pass_three = Convert.ToInt32(numToPassThree.Text)
                };
                MainClass.db.Tests.Add(test);
                MainClass.db.SaveChanges();
                var addedTestVar = MainClass.db.Tests.Where(x => x.Test_name == testName.Text).First();
                MainClass.addedTestID = addedTestVar.id;
                MainClass.addedTestName = testName.Text;
                MainClass.FrameVar.Navigate(new AddQuestions());
            }
            else
                MessageBox.Show("Заполните все поля", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }
}
