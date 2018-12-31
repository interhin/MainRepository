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
    /// Interaction logic for AddQuestions.xaml
    /// </summary>
    public partial class AddQuestions : Page
    {
        string unselectedText = "Не выбран";
        public AddQuestions()
        {
            InitializeComponent();
        }

        private void addOptionBut_Click(object sender, RoutedEventArgs e)
        {
            optionsList.Items.Add(optionName.Text);
        }

        private void delOptionBut_Click(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show("Вы уверены что хотите удалить этот вариант?", "Внимание", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.Yes)
            {
                if (optionsList.SelectedItem.ToString() == correctAnswerText.Text)
                    correctAnswerText.Text = unselectedText;
                optionsList.Items.Remove(optionsList.SelectedItem);
                turnOffButtons();
            }
        }

        private void correctAnswerBut_Click(object sender, RoutedEventArgs e)
        {
            correctAnswerText.Text = optionsList.SelectedItem.ToString();
        }

        private void addQuestion_Click(object sender, RoutedEventArgs e)
        {
            if (questionNameText.Text != "" && optionsList.Items != null && correctAnswerText.Text != unselectedText)
            {
                Questions question = new Questions()
                {
                    Question = questionNameText.Text,
                    Answer_id = null,
                    Test_id = MainClass.addedTestID

                };
                MainClass.db.Questions.Add(question);
                MainClass.db.SaveChanges();

                foreach (var item in optionsList.Items)
                {
                    Options option = new Options() {
                        Text = item.ToString(),
                        QuestionID = question.id
                    };
                    MainClass.db.Options.Add(option);
                    MainClass.db.SaveChanges();

                    question.Answer_id = option.id;
                    MainClass.db.SaveChanges();
                }
                if (MessageBox.Show("Вопрос успешно добавлен \n Вы желаете добавить еще вопрос?", "Информация", MessageBoxButton.YesNo, MessageBoxImage.Information) == MessageBoxResult.Yes)
                {

                    optionsList.Items.Clear();
                    questionNameText.Text = "";
                    correctAnswerText.Text = unselectedText;
                    optionName.Text = "";
                    turnOffButtons();
                }
                else
                    MainClass.FrameVar.Navigate(new MenuPanel());

            }
            else
                MessageBox.Show("Заполните все поля", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);

        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            testNameHeader.Text = MainClass.addedTestName;
        }

        private void exitToMenu_Click(object sender, RoutedEventArgs e)
        {
            MainClass.FrameVar.Navigate(new MenuPanel());
        }

        void turnOffButtons()
        {
            delOptionBut.IsEnabled = false;
            correctAnswerBut.IsEnabled = false;
        }

        void turnOnButtons()
        {
            delOptionBut.IsEnabled = true;
            correctAnswerBut.IsEnabled = true;
        }

        private void optionsList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            turnOnButtons();
        }
    }
}
