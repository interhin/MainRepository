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
    public partial class testingPage : Page
    {
        DispatcherTimer passTimer = new DispatcherTimer();
        DispatcherTimer questionTimer = new DispatcherTimer();
        int currentQuestionID = -1;
        int passTime = 0;
        int questionTime = 0;
        int questionTimeVar = 0;
        int numToFive = 0;
        int numToFour = 0;
        int numToThree = 0;
        int totalBalls = 0;
        int answerID = 0;

        public int selectedValueID { get; set; }

        List<Questions> questionsList = new List<Questions>();
        List<Options> options = new List<Options>();
        public testingPage()
        {
            InitializeComponent();
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            testNameTB.Text = MainClass.studentTestingName;

            loadQuestionF();
            loadVars();

            passTimer.Tick += PassTimer_Tick;
            questionTimer.Tick += QuestionTimer_Tick;
            passTimer.Interval = new TimeSpan(0,0,1);
            questionTimer.Interval = new TimeSpan(0,0,1);
        }

        private void QuestionTimer_Tick(object sender, EventArgs e)
        {
            questionTime--;
            if (questionTime != 0)
            {
                questionTimeTB.Text = String.Format("Времени на тест: {0} часов {1} минут {2} секунд",
                    (questionTime / 3600),
                    ((questionTime % 3600) / 60),
                    ((questionTime % 3600) % 60));
            }
            else
            {
                loadQuestionF();
            }
        }

        private void PassTimer_Tick(object sender, EventArgs e)
        {
            passTime--;
            if (passTime != 0)
            {
                passTimeTB.Text = String.Format("Времени на вопрос: {0} часов {1} минут {2} секунд",
                    (passTime / 3600),
                    ((passTime % 3600) / 60),
                    ((passTime % 3600) % 60));
            }
            else
            {
                endTest();
            }
        }

        void loadQuestionF()
        {
            currentQuestionID++;
            questionTime = questionTimeVar;
            questionsList = MainClass.db.Questions.Where(x => x.Test_id == MainClass.studentTestingID).ToList();
            if (currentQuestionID > questionsList.Count)
            {
                MessageBox.Show("Тебе сюда нельзя!", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                MainClass.FrameVar.Navigate(new StudentsPanel());
            }
            else
            {
                if (currentQuestionID != questionsList.Count)
                {
                    var qID = questionsList[currentQuestionID].id;
                    questionTB.Text = questionsList[currentQuestionID].Question;
                    answerID = questionsList[currentQuestionID].Answer_id ?? 0;
                    options = MainClass.db.Options.Where(x => x.QuestionID == qID).ToList();
                    optionsList.ItemsSource = options;
                }
                else
                {
                    endTest();
                    int ball = 0;
                    if (totalBalls >= numToFive)
                        ball = 5;
                    else if (totalBalls >= numToFour)
                        ball = 4;
                    else if (totalBalls >= numToThree)
                        ball = 3;
                    else
                        ball = 2;

                    History history = new History() {
                        User_id = CurrentUser.curUser.id,
                        Test_id = MainClass.studentTestingID,
                        Date = DateTime.Now,
                        Ball = ball
                    };
                    MainClass.db.History.Add(history);
                    MainClass.db.SaveChanges();
                    MessageBox.Show(String.Format("Тест окончен, правильных ответов: {0}, ваша оценка за тест: {1}", totalBalls, ball), "Информация", MessageBoxButton.OK, MessageBoxImage.Information);
                    MainClass.FrameVar.Navigate(new StudentsPanel());
                }
            }
        }

        void loadVars()
        {
            var _test = MainClass.db.Tests.Where(x => x.id == MainClass.studentTestingID).First();
            questionTimeVar = (int)_test.Question_time;
            passTime = (int)_test.Pass_time;
            questionTime = questionTimeVar;

            passTimer.Start();
            questionTimer.Start();

            numToFive = _test.Num_to_pass_five;
            numToFour = _test.Num_to_pass_four;
            numToThree = _test.Num_to_pass_three;
        }

        void endTest()
        {
            questionsList = null;
            options = null;
            passTimer.Stop();
            questionTimer.Stop();
        }

        private void nextQuestBut_Click(object sender, RoutedEventArgs e)
        {
            if (selectedValueID == answerID)
                totalBalls++;
            loadQuestionF();
        }
    }
}
