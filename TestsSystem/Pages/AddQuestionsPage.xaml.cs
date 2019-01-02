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
    public partial class AddQuestionsPage : Page
    {
        int correctAnswerIndex = -1;

        public AddQuestionsPage()
        {
            InitializeComponent();
        }

        private void addOptionBut_Click(object sender, RoutedEventArgs e)
        {
            // Добавляем вариант ответа в список
            if (!String.IsNullOrWhiteSpace(optionNameTBox.Text))
                optionsLB.Items.Add(optionNameTBox.Text);
        }

        private void delOptionBut_Click(object sender, RoutedEventArgs e)
        {
            if (MessageService.ShowYesNoWarning("Вы уверены что хотите удалить этот вариант?") == MessageBoxResult.Yes)
            {
                // Если удаляем вариант который был правильным ответом то меняем статус на "Не выбрано"
                if (optionsLB.SelectedIndex == correctAnswerIndex)
                {
                    correctAnswerIndex = -1;
                    MainClass.DisableGlowAllItems(ref optionsLB);
                }
                // Если нет то просто удаляем и отключаем кнопки
                optionsLB.Items.Remove(optionsLB.SelectedItem);
                DisableButtons();
            }
        }

        private void correctAnswerBut_Click(object sender, RoutedEventArgs e)
        {
            // Меняем правильный ответ
            if (optionsLB.SelectedItem != null)
                correctAnswerIndex = optionsLB.SelectedIndex;

            MainClass.DisableGlowAllItems(ref optionsLB);
            MainClass.GlowSelectedItem(ref optionsLB);
        }

        private void addQuestionBut_Click(object sender, RoutedEventArgs e)
        {
            // Если все поля заполнены то добавляем вопрос
            if (!String.IsNullOrWhiteSpace(questionTBox.Text) && optionsLB.Items != null && correctAnswerIndex != -1)
            {
                try
                {
                    AddQuestion();
                } catch (Exception ex)
                {
                    MessageService.ShowError("Ошибка при добавлении вопроса: "+ex.Message);
                    ClearFields();
                    return;
                }

                if (MessageService.ShowYesNoInfo("Вопрос успешно добавлен \n Вы желаете добавить еще вопрос?") == MessageBoxResult.Yes)
                {
                    // Очищаем все поля и отключаем кнопки
                    ClearFields();
                    DisableButtons();
                }
                else
                    MainClass.FrameVar.Navigate(new TeachersMenuPage());  // Возвращаемся в меню

            }
            else
                MessageService.ShowError("Заполните все поля!");

        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            testNameTBl.Text = TestsService.addedTest.Name; // Выводим имя теста к которому добавляем вопросы
        }

        private void exitToMenuBut_Click(object sender, RoutedEventArgs e)
        {
            MainClass.FrameVar.Navigate(new TeachersMenuPage());
        }

        private void optionsLB_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            EnableButtons();
        }

        void DisableButtons()
        {
            delOptionBut.IsEnabled = false;
            correctAnswerBut.IsEnabled = false;
        }

        void EnableButtons()
        {
            delOptionBut.IsEnabled = true;
            correctAnswerBut.IsEnabled = true;
        }

        void ClearFields()
        {
            optionsLB.Items.Clear();
            questionTBox.Text = "";
            optionNameTBox.Text = "";
        }

        void AddQuestion()
        {
            int correctAnswerDbID = 0; // Переменная для ID правильного ответ (который присвоит база данных)
            Questions question = new Questions()
            {
                Text = questionTBox.Text,
                AnswerID = null,
                TestID = TestsService.addedTest.id

            };
            MainClass.db.Questions.Add(question);
            MainClass.db.SaveChanges();

            // Добавляем варианты ответа в соответствующую таблицу
            for (int i = 0;i<optionsLB.Items.Count;i++)
            {
                Options option = new Options()
                {
                    Text = optionsLB.Items[i].ToString(),
                    QuestionID = question.id
                };
                MainClass.db.Options.Add(option);
                MainClass.db.SaveChanges();

                if (i == correctAnswerIndex) // Запоминаем полученный ID для правильного ответа
                    correctAnswerDbID = option.id;
            }

            question.AnswerID = correctAnswerDbID;
            MainClass.db.SaveChanges();
        }
    }
}
