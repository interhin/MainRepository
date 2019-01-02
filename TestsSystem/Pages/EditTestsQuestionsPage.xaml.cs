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
        Questions selectedQuestion = null;
        Options selectedOption = null;
        string unselectedQuestion = "Вы не выбрали вопрос!";
        string unselectedOption = "Вы не выбрали вариант ответа";
        string unselectedQuestOrOption = "Вы не выбрали вопрос или вариант ответа!";

        public EditTestsQuestionsPage()
        {
            InitializeComponent();
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            // Загружаем все вопросы теста
            questionsLB.DisplayMemberPath = "Question";
            questionsLB.SelectedValuePath = "id";
            optionsLB.DisplayMemberPath = "Text";
            optionsLB.SelectedValuePath = "id";
            try
            {
                LoadQuestions();
            } catch
            {
                MessageService.ShowError("Ошибка при загрузке вопросов!");
            }
        }

        private void questionsLB_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            selectedQuestion = questionsLB.SelectedItem as Questions;
            // Исключение на случай если удаляем вопрос (это событие срабатывает когда выбранный вопрос удаляется)
            if (selectedQuestion != null)
            {
                LoadSelQuestOptions(); // Загружаем варианты ответа выбранного вопроса
                editQuestionTBox.Text = selectedQuestion.Question; // Выводим в поле изменения сам вопрос
                addOptionBut.IsEnabled = true; // Включаем кнопку добавить вариант ответа
                EnableQuestionsBut(); // Включаем кнопки изменить и удалить вопрос
            }
            selectedOption = null;
            DisableOptionsBut(); // Отключаем кнопки изменить, удалить и сделать правильным т.к. при смене вопроса вариант не выбран
        }


        private void optionsLB_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            selectedOption = optionsLB.SelectedItem as Options;
            if (selectedOption != null)
            {
                EnableOptionsBut(); // Включаем кнопки изменить, удалить и сделать правильным

                editOptionTBox.Text = selectedOption.Text;  // Выводим в поле измения сам вариант ответа
            }
        }

        private void delQuestionBut_Click(object sender, RoutedEventArgs e)
        {
            if (MessageService.ShowYesNoWarning("Вы уверены что хотите удалить этот вопрос?") == MessageBoxResult.Yes)
            {
                // Удаляем выбранный вопрос
                if (selectedQuestion != null)
                {
                    MainClass.db.Questions.Attach(selectedQuestion);
                    MainClass.db.Questions.Remove(selectedQuestion);
                    MainClass.db.SaveChanges();

                    LoadQuestions(); // Обновляем список вопросов
                }
                else
                    MessageService.ShowWarning(unselectedQuestion);
            }
        }

        private void addQuestionBut_Click(object sender, RoutedEventArgs e)
        {
            // Добавляем вопрос с введнным именем
            if (!String.IsNullOrWhiteSpace(questionNameTBox.Text))
            {
                Questions question = new Questions()
                {
                    Question = questionNameTBox.Text,
                    Answer_id = null,
                    Test_id = TestsService.editingTest.id
                };
                MainClass.db.Questions.Add(question);
                MainClass.db.SaveChanges();

                LoadQuestions(); // Обновляем список вопросов
            }
        }

        private void delOptionBut_Click(object sender, RoutedEventArgs e)
        {
            if (MessageService.ShowYesNoWarning("Вы уверены что хотите удалить этот вариант ответа?") == MessageBoxResult.Yes)
            {
                // Удаляем выбранный вариант ответа
                if (selectedOption != null)
                {
                    MainClass.db.Options.Attach(selectedOption);
                    MainClass.db.Options.Remove(selectedOption);
                    MainClass.db.SaveChanges();
                    LoadSelQuestOptions(); // Обновляем список вариантов ответа
                    DisableOptionsBut(); // Отключаем кнопки Удалить и Сделать правильным
                }
                else
                    MessageService.ShowWarning(unselectedOption);
            }
        }

        private void editQuestionBut_Click(object sender, RoutedEventArgs e)
        {
            // Изменяем текст вопроса
            if (selectedQuestion != null)
            {
                if (!String.IsNullOrWhiteSpace(editQuestionTBox.Text))
                {
                    selectedQuestion.Question = editQuestionTBox.Text;
                    MainClass.db.SaveChanges();

                    LoadQuestions();
                }
            }
            else
                MessageService.ShowWarning(unselectedQuestion);
        }

        private void addOptionBut_Click(object sender, RoutedEventArgs e)
        {
            // Добавляем вариант ответа к выбранному вопросу
            if (selectedQuestion != null)
            {
                if (!String.IsNullOrWhiteSpace(optionNameTBox.Text))
                {
                    var optionVar = MainClass.db.Options.Add(new Options()
                    {
                        Text = optionNameTBox.Text,
                        QuestionID = selectedQuestion.id
                    });
                    MainClass.db.SaveChanges();

                    LoadSelQuestOptions(); // Обновляем список вариантов ответа
                }
            }
            else
                MessageService.ShowWarning(unselectedQuestion);
        }

        private void makeCorrectBut_Click(object sender, RoutedEventArgs e)
        {
            // Делаем выбранный вариант ответа правильным
            if (selectedQuestion != null && selectedOption != null)
            {
                MainClass.db.Questions.Attach(selectedQuestion);
                selectedQuestion.Answer_id = selectedOption.id;
                MainClass.db.SaveChanges();
                MessageService.ShowInfo("Выбранный вариант помечен как правильный!");
                LoadSelQuestOptions();
            }
            else
                MessageService.ShowWarning(unselectedQuestOrOption);
        }


        private void editOptionBut_Click(object sender, RoutedEventArgs e)
        {
            // Изменяем текст варианта ответа
            if (selectedOption != null)
            {
                if (!String.IsNullOrWhiteSpace(editOptionTBox.Text))
                {
                    MainClass.db.Options.Attach(selectedOption);
                    selectedOption.Text = editOptionTBox.Text;
                    MainClass.db.SaveChanges();

                    LoadSelQuestOptions();
                }
            }
            else
                MessageService.ShowWarning(unselectedOption);
        }

        // Отключения и включения кнопок

        void EnableOptionsBut()
        {
            delOptionBut.IsEnabled = true;
            editOptionBut.IsEnabled = true;
            makeCorrectBut.IsEnabled = true;
        }

        void DisableOptionsBut()
        {
            delOptionBut.IsEnabled = false;
            editOptionBut.IsEnabled = false;
            makeCorrectBut.IsEnabled = false;
        }

        void EnableQuestionsBut()
        {
            delQuestionBut.IsEnabled = true;
            editQuestionBut.IsEnabled = true;
        }

        void DisableQuestionsBut()
        {
            delQuestionBut.IsEnabled = false;
            editQuestionBut.IsEnabled = false;
        }

        void LoadQuestions()
        {
            // Загрузка всех вопросов выбранного теста
            questionsLB.ItemsSource = MainClass.db.Questions.Where(x => x.Test_id == TestsService.editingTest.id).ToList();
        }

        void LoadSelQuestOptions()
        {
            // Загрузка всех вариантов ответа выбранного вопроса
            optionsLB.ItemsSource = MainClass.db.Options.Where(x => x.QuestionID == selectedQuestion.id).ToList();
            GlowCorrectItem();
        }

        void GlowCorrectItem()
        {
            if (selectedQuestion != null)
            {
                for (int i = 0; i < optionsLB.Items.Count; i++)
                {
                    if ((optionsLB.Items[i] as Options).id == selectedQuestion.Answer_id)
                    {
                        optionsLB.UpdateLayout(); // ?? (Нашёл в инете решение, без него ContainerFromItem возвращает null)
                        var correctStyle = Application.Current.FindResource("correctAnswerStyle") as Style;
                        (optionsLB.ItemContainerGenerator.ContainerFromItem(optionsLB.Items[i]) as ListBoxItem).Style = correctStyle;
                    }
                }
            }
        }
    }
}
