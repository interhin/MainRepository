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
            // Загружаем все вопросы теста
            LoadQuestions();
        }

        private void questionsLB_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            // Если мы выбрали другой вопрос то подгружаем его варианты, заполняем поле Изменить и отключаем кнопки
            LoadSelQuestOptions();
            addOptionBut.IsEnabled = true;
            TurnOnQuestionsBut();
            TurnOffOptionsBut();
            if (questionsLB.SelectedItem != null)
                editQuestionTBox.Text = (questionsLB.SelectedItem as Questions).Question;
        }


        private void optionsLB_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            // Включаем кнопки если был выбран вариант ответа и заполняем поле Изменить
            TurnOnOptionsBut();
            if (optionsLB.SelectedItem != null)
                editOptionTBox.Text = (optionsLB.SelectedItem as Options).Text;
        }

        private void delQuestionBut_Click(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show("Вы уверены что хотите удалить этот вопрос?","Внимание",MessageBoxButton.YesNo,MessageBoxImage.Warning) == MessageBoxResult.Yes)
            {
                // Удаляем выбранный вопрос
                int selId = Convert.ToInt32(questionsLB.SelectedValue);
                var selQuest = MainClass.db.Questions.Where(x => x.id == selId).First();
                MainClass.db.Questions.Remove(selQuest);
                MainClass.db.SaveChanges();

                LoadQuestions(); // Обновляем список вопросов
            }
        }

        private void addQuestionBut_Click(object sender, RoutedEventArgs e)
        {
            // Добавляем вопрос с введнным именем
            Questions questionVar = new Questions()
            {
                Question = questionNameTBox.Text,
                Answer_id = null,
                Test_id = MainClass.editingTestID
            };
            MainClass.db.Questions.Add(questionVar);
            MainClass.db.SaveChanges();

            LoadQuestions(); // Обновляем список вопросов
        }

        private void delOptionBut_Click(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show("Вы уверены что хотите удалить этот вариант ответа?", "Внимание", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.Yes)
            {
                // Удаляем выбранный вариант ответа
                int selId = Convert.ToInt32(optionsLB.SelectedValue);
                var selOption = MainClass.db.Options.Where(x => x.id == selId).First();
                MainClass.db.Options.Remove(selOption);
                MainClass.db.SaveChanges();
                LoadSelQuestOptions(); // Обновляем список вариантов ответа
                TurnOffOptionsBut(); // Отключаем кнопки Удалить и Сделать правильным
            }
        }

        private void editQuestionBut_Click(object sender, RoutedEventArgs e)
        {
            // Изменяем текст вопроса
            (questionsLB.SelectedItem as Questions).Question = editQuestionTBox.Text;
            MainClass.db.SaveChanges();

            LoadQuestions();
        }

        private void addOptionBut_Click(object sender, RoutedEventArgs e)
        {
            // Добавляем вариант ответа к выбранному вопросу
            int selId = Convert.ToInt32(questionsLB.SelectedValue);
            var optionVar = MainClass.db.Options.Add(new Options()
            {
                Text = optionNameTBox.Text,
                QuestionID = selId
            });
            MainClass.db.SaveChanges();

            LoadSelQuestOptions(); // Обновляем список вариантов ответа
        }

        private void makeCorrectBut_Click(object sender, RoutedEventArgs e)
        {
            // Делаем выбранный вариант ответа правильным
            int selId = Convert.ToInt32(questionsLB.SelectedValue);
            var selQuest = MainClass.db.Questions.Where(x => x.id == selId).First();
            selQuest.Answer_id = Convert.ToInt32(optionsLB.SelectedValue);
            MainClass.db.SaveChanges();
            MessageBox.Show("Выбранный вариант помечен как правильный!", "Информация", MessageBoxButton.OK, MessageBoxImage.Information);
        }


        private void editOptionBut_Click(object sender, RoutedEventArgs e)
        {
            // Изменяем текст варианта ответа
            (optionsLB.SelectedItem as Options).Text = editOptionTBox.Text;
            MainClass.db.SaveChanges();

            LoadSelQuestOptions();
        }

        // Отключения и включания кнопок

        void TurnOnOptionsBut()
        {
            delOptionBut.IsEnabled = true;
            editOptionBut.IsEnabled = true;
            makeCorrectBut.IsEnabled = true;
        }

        void TurnOffOptionsBut()
        {
            delOptionBut.IsEnabled = false;
            editOptionBut.IsEnabled = false;
            makeCorrectBut.IsEnabled = false;
        }

        void TurnOnQuestionsBut()
        {
            delQuestionBut.IsEnabled = true;
            editQuestionBut.IsEnabled = true;
        }

        void TurnOffQuestionsBut()
        {
            delQuestionBut.IsEnabled = false;
            editQuestionBut.IsEnabled = false;
        }

        void LoadQuestions()
        {
            // Загрузка всех вопросов выбранного теста
            questionsLB.DisplayMemberPath = "Question";
            questionsLB.SelectedValuePath = "id";
            questionsLB.ItemsSource = MainClass.db.Questions.Where(x => x.Test_id == MainClass.editingTestID).ToList();
        }

        void LoadSelQuestOptions()
        {
            // Загрузка всех вариантов ответа выбранного вопроса
            int selId = Convert.ToInt32(questionsLB.SelectedValue);
            if (selId != 0)
            {
                var optionsListVar = MainClass.db.Options.Where(x => x.QuestionID == selId).ToList();
                optionsLB.DisplayMemberPath = "Text";
                optionsLB.SelectedValuePath = "id";
                optionsLB.ItemsSource = optionsListVar; // Обновляем список вопросов
            }
        }
    }
}
