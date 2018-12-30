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
    /// Interaction logic for EditTestsQuestionsPage.xaml
    /// </summary>
    public partial class EditTestsQuestionsPage : Page
    {
        public EditTestsQuestionsPage()
        {
            InitializeComponent();
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            loadQuestionF();
        }

        private void questionsList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            loadSelQuestOptions();
            addOptionBut.IsEnabled = true;
            turnOnQuestionsBut();
            turnOffOptionsBut();
            if (questionsList.SelectedItem != null)
                editQuestionTextBox.Text = (questionsList.SelectedItem as Questions).Question;
        }

        private void delQuestionBut_Click(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show("Вы уверены что хотите удалить этот вопрос?","Внимание",MessageBoxButton.YesNo,MessageBoxImage.Warning) == MessageBoxResult.Yes)
            {
                int selId = Convert.ToInt32(questionsList.SelectedValue);
                var selQuest = MainClass.db.Questions.Where(x => x.id == selId).First();
                MainClass.db.Questions.Remove(selQuest);
                MainClass.db.SaveChanges();
                optionsList.ItemsSource = null;
                loadQuestionF();
            }
        }

        private void addQuestionBut_Click(object sender, RoutedEventArgs e)
        {
            Questions questionVar = new Questions()
            {
                Question = questionTextBox.Text,
                Answer_id = null,
                Test_id = MainClass.editingTestId
            };
            MainClass.db.Questions.Add(questionVar);
            MainClass.db.SaveChanges();
            loadQuestionF();
        }

        private void delOptionBut_Click(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show("Вы уверены что хотите удалить этот вариант ответа?", "Внимание", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.Yes)
            {
                int selId = Convert.ToInt32(optionsList.SelectedValue);
                var selOption = MainClass.db.Options.Where(x => x.id == selId).First();
                MainClass.db.Options.Remove(selOption);
                MainClass.db.SaveChanges();
                loadSelQuestOptions();
                turnOffOptionsBut();
            }
        }

        void loadQuestionF()
        {
            questionsList.DisplayMemberPath = "Question";
            questionsList.SelectedValuePath = "id";
            questionsList.ItemsSource = MainClass.db.Questions.Where(x => x.Test_id == MainClass.editingTestId).ToList();
        }

        void loadSelQuestOptions()
        {
            int selId = Convert.ToInt32(questionsList.SelectedValue);
            if (selId != 0)
            {
                var optionsListVar = MainClass.db.Options.Where(x => x.QuestionID == selId).ToList();
                optionsList.DisplayMemberPath = "Text";
                optionsList.SelectedValuePath = "id";
                optionsList.ItemsSource = optionsListVar;
            }
        }

        private void addOptionBut_Click(object sender, RoutedEventArgs e)
        {
            int selId = Convert.ToInt32(questionsList.SelectedValue);
            var optionVar = MainClass.db.Options.Add(new Options() {
                Text = optionTextBox.Text,
                QuestionID = selId
            });
            MainClass.db.SaveChanges();
            loadSelQuestOptions();
        }

        private void optionsList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            turnOnOptionsBut();
            if (optionsList.SelectedItem != null)
                editOptionTextBox.Text = (optionsList.SelectedItem as Options).Text;
        }

        private void editQuestionBut_Click(object sender, RoutedEventArgs e)
        {
            (questionsList.SelectedItem as Questions).Question = editQuestionTextBox.Text;
            MainClass.db.SaveChanges();
            loadQuestionF();
        }

        private void editOptionBut_Click(object sender, RoutedEventArgs e)
        {
            (optionsList.SelectedItem as Options).Text = editOptionTextBox.Text;
            MainClass.db.SaveChanges();
            loadSelQuestOptions();
        }

        void turnOnOptionsBut()
        {
            delOptionBut.IsEnabled = true;
            editOptionBut.IsEnabled = true;
            makeCorrectBut.IsEnabled = true;
        }

        void turnOffOptionsBut()
        {
            delOptionBut.IsEnabled = false;
            editOptionBut.IsEnabled = false;
            makeCorrectBut.IsEnabled = false;
        }

        void turnOnQuestionsBut()
        {
            delQuestionBut.IsEnabled = true;
            editQuestionBut.IsEnabled = true;
        }

        void turnOffQuestionsBut()
        {
            delQuestionBut.IsEnabled = false;
            editQuestionBut.IsEnabled = false;
        }

        private void makeCorrectBut_Click(object sender, RoutedEventArgs e)
        {
            int selId = Convert.ToInt32(questionsList.SelectedValue);
            var selQuest = MainClass.db.Questions.Where(x => x.id == selId).First();
            selQuest.Answer_id = Convert.ToInt32(optionsList.SelectedValue);
            MainClass.db.SaveChanges();
            MessageBox.Show("Выбранный вариант помечен ответом!","Информация",MessageBoxButton.OK,MessageBoxImage.Information);
        }
    }
}
