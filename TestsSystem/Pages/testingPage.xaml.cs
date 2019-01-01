using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Threading;
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
    /// Interaction logic for testingPage.xaml
    /// </summary>
    public partial class TestingPage : Page
    {
        // Таймеры для теста и вопроса
        DispatcherTimer passTimer = new DispatcherTimer();
        DispatcherTimer questionTimer = new DispatcherTimer();

        // Счетчики и переменные теста
        int currentQuestionNum = -1; // Счетчик текущего вопроса (начиная с 0)
        int passTime = 0;
        int questionTime = 0;
        int QUESTION_TIME = 0;
        int numToFive = 0;
        int numToFour = 0;
        int numToThree = 0;
        int totalBalls = 0;
        int answerID = 0;

        // Выбранный вариант ответа
        public int selectedValueID { get; set; }

        // Список с вопросами
        List<Questions> questionsList = new List<Questions>();

        public TestingPage()
        {
            InitializeComponent();
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {

            // Загрузка данных о тесте и первого вопроса
            LoadTestInfo();
            LoadQuestion(++currentQuestionNum);

            SetTimers(); // Настройка таймеров
            StartTimers();
        }

        private void QuestionTimer_Tick(object sender, EventArgs e)
        {
            // Таймер вопроса
            questionTime--;
            if (questionTime > 0)
                UpdateQuestionTBox();
            else
            {
                ResetQuestionTime();
                LoadQuestion(++currentQuestionNum);
            }
        }

        private void PassTimer_Tick(object sender, EventArgs e)
        {
            // Таймер теста
            passTime--;
            if(passTime > 0)
                UpdatePassTBox();
            else
                CompleteTest();
        }

        private void nextQuestBut_Click(object sender, RoutedEventArgs e)
        {
            // Если ответ верный прибавляем баллы
            if (selectedValueID == answerID)
                totalBalls++;
            ResetQuestionTime();
            LoadQuestion(++currentQuestionNum);
        }

        void LoadQuestion(int questionNum)
        {
            // Если кто-либо нажмет Назад после прохождения теста
            if (questionNum > questionsList.Count)
            {
                MessageBox.Show("Тебе сюда нельзя!", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                MainClass.FrameVar.Navigate(new StudentsMenuPage());
            }
            else if (questionNum < questionsList.Count) // Смотрим не вышли ли мы за пределы массива
            {
                    var currentQuestionID = questionsList[questionNum].id; // Узнаем ID вопроса чтобы найти его варианты ответа
                    questionTBl.Text = questionsList[questionNum].Question; // Сам вопрос
                    answerID = questionsList[questionNum].Answer_id ?? 0; // Запоминаем ID правильного ответа
                    optionsLB.ItemsSource = MainClass.db.Options.Where(x => x.QuestionID == currentQuestionID).ToList(); // Грузим список в ListBox
            }
            else
            {
            CompleteTest();
            }
        }

        void LoadTestInfo()
        {
            // Загружаем вопросы текущего теста
            questionsList = MainClass.db.Questions.Where(x => x.Test_id == MainClass.testingTestID).ToList();

            // Загружаем данные о тесте
            var curTest = MainClass.db.Tests.Where(x => x.id == MainClass.testingTestID).First();
            questionTime = QUESTION_TIME = (int)curTest.Question_time;
            passTime = (int)curTest.Pass_time;

            numToFive = curTest.Num_to_pass_five;
            numToFour = curTest.Num_to_pass_four;
            numToThree = curTest.Num_to_pass_three;

            // Вывод в заголовок имя теста
            testNameTBl.Text = MainClass.testingTestName;

            UpdatePassTBox();
            UpdateQuestionTBox();
        }

        void CompleteTest()
        {
            questionsList = null;
            passTimer.Stop();
            questionTimer.Stop();

            int ball = 0;
            if (totalBalls >= numToFive)
                ball = 5;
            else if (totalBalls >= numToFour)
                ball = 4;
            else if (totalBalls >= numToThree)
                ball = 3;
            else
                ball = 2;

            History history = new History()
            {
                User_id = CurrentUser.curUser.id,
                Test_id = MainClass.testingTestID,
                Date = DateTime.Now,
                Ball = ball
            };
            MainClass.disableBack = true;
            MainClass.db.History.Add(history);
            MainClass.db.SaveChanges();
            MessageBox.Show(String.Format("Тест окончен, правильных ответов: {0}, ваша оценка за тест: {1}", totalBalls, ball), "Информация", MessageBoxButton.OK, MessageBoxImage.Information);
            MainClass.FrameVar.Navigate(new StudentsMenuPage());
        }

        void SetTimers()
        {
            // Подписка на события при тике и установка интервалов (1 секунда)
            passTimer.Tick += PassTimer_Tick;
            questionTimer.Tick += QuestionTimer_Tick;
            passTimer.Interval = new TimeSpan(0, 0, 1);
            questionTimer.Interval = new TimeSpan(0, 0, 1);
        }

        void StartTimers()
        {
            passTimer.Start();
            questionTimer.Start();
        }

        void UpdatePassTBox()
        {
            passTimeTBl.Text = String.Format("Времени на вопрос: {0} часов {1} минут {2} секунд",
                    (passTime / 3600),
                    ((passTime % 3600) / 60),
                    ((passTime % 3600) % 60));
        }

        void UpdateQuestionTBox()
        {
            questionTimeTBl.Text = String.Format("Времени на тест: {0} часов {1} минут {2} секунд",
                    (questionTime / 3600), // Часы
                    ((questionTime % 3600) / 60), // Минуты
                    ((questionTime % 3600) % 60)); // Секунды
        }

        void ResetQuestionTime()
        {
            questionTime = QUESTION_TIME;
            UpdateQuestionTBox();
        }

    }
}
