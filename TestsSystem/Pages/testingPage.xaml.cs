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
        int totalBalls = 0; // Счетчик правильных ответов
        int answerID = 0;

        // Выбранный вариант ответа (Привязан Binding'ом)
        public int selectedOptionID { get; set; } = -1;

        // Список с вопросами
        List<Questions> questionsList = new List<Questions>();

        public TestingPage()
        {
            InitializeComponent();
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {

            // Загрузка данных о тесте и первого вопроса
            if (LoadTestInfo())
            {
                LoadQuestion(++currentQuestionNum);

                SetTimers(); // Настройка таймеров
                StartTimers();
            }
            else
                MessageService.ShowError("Ошибка при загрузке информации о тесте");
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
            if (selectedOptionID != -1)
            {
                // Если ответ верный прибавляем баллы
                if (selectedOptionID == answerID)
                    totalBalls++;
                // Сбрасываем счетчик времени ответа на вопрос и загружаем следующий вопрос
                ResetQuestionTime();
                LoadQuestion(++currentQuestionNum);
            }
            else
                MessageService.ShowWarning("Вы не выбрали вариант ответа");
        }

        void LoadQuestion(int questionNum)
        {
            if (questionNum < questionsList.Count) // Смотрим не вышли ли мы за пределы массива
            {
                    var currentQuestionID = questionsList[questionNum].id; // Узнаем ID вопроса чтобы найти его варианты ответа
                    questionTBl.Text = questionsList[questionNum].Question; // Сам вопрос
                    answerID = questionsList[questionNum].Answer_id ?? 0; // Запоминаем ID правильного ответа
                    optionsLB.ItemsSource = MainClass.db.Options.Where(x => x.QuestionID == currentQuestionID).ToList(); // Грузим список в ListBox
            }
            else
                CompleteTest();
        }

        bool LoadTestInfo()
        {
            try {
                // Загружаем вопросы текущего теста
                questionsList = MainClass.db.Questions.Where(x => x.Test_id == TestsService.testingTest.id).ToList();
            }
            catch
            {
                return false;
            }

            // Загружаем данные о тесте
            questionTime = TestsService.testingTest.Question_time;
            passTime = TestsService.testingTest.Pass_time;

            // Вывод в заголовок имя теста
            testNameTBl.Text = TestsService.testingTest.Test_name;

            UpdatePassTBox();
            UpdateQuestionTBox();
            return true;
        }

        void CompleteTest()
        {

            int ball = CalcBall(totalBalls);

            if (InsertToHistory(ball))
            {
                questionsList = null;
                StopTimers();

                MessageService.ShowInfo(String.Format("Тест окончен, правильных ответов: {0}, ваша оценка за тест: {1}", totalBalls, ball));

                MainClass.disableBack = true; // Отключаем кнопку назад чтобы нельзя было пройти тест заного
                MainClass.FrameVar.Navigate(new StudentsMenuPage());
            }
            else
                MessageService.ShowError("Произошла ошибка при завершении теста!");
        }

        int CalcBall(int totalBalls)
        {
            int ball = 0;

            if (totalBalls >= TestsService.testingTest.Num_to_pass_five)
                ball = 5;
            else if (totalBalls >= TestsService.testingTest.Num_to_pass_four)
                ball = 4;
            else if (totalBalls >= TestsService.testingTest.Num_to_pass_three)
                ball = 3;
            else
                ball = 2;

            return ball;
        }

        bool InsertToHistory(int ball)
        {
            try
            {
                History history = new History()
                {
                    User_id = UsersService.currentUser.id,
                    Test_id = TestsService.testingTest.id,
                    Date = DateTime.Now,
                    Ball = ball
                };
                MainClass.db.History.Add(history);
                MainClass.db.SaveChanges();

                return true;
            } catch
            {
                return false;
            }
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

        void StopTimers()
        {
            passTimer.Stop();
            questionTimer.Stop();
        }

        void UpdatePassTBox()
        {
            passTimeTBl.Text = String.Format("Времени на вопрос: {0} часов {1} минут {2} секунд",
                    CalcService.CalcHours(passTime),
                    CalcService.CalcMinutes(passTime),
                    CalcService.CalcSeconds(passTime));
        }

        void UpdateQuestionTBox()
        {
            questionTimeTBl.Text = String.Format("Времени на тест: {0} часов {1} минут {2} секунд",
                    CalcService.CalcHours(questionTime),
                    CalcService.CalcMinutes(questionTime),
                    CalcService.CalcSeconds(questionTime));
        }

        void ResetQuestionTime()
        {
            questionTime = TestsService.testingTest.Question_time;
            UpdateQuestionTBox();
        }

    }
}
