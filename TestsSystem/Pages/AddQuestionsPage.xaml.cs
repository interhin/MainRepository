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
        string unselectedText = "Не выбран";

        public AddQuestionsPage()
        {
            InitializeComponent();
        }

        private void addOptionBut_Click(object sender, RoutedEventArgs e)
        {
            // Добавляем вариант ответа в список
            optionsLB.Items.Add(optionNameTBox.Text);
        }

        private void delOptionBut_Click(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show("Вы уверены что хотите удалить этот вариант?", "Внимание", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.Yes)
            {
                // Если удаляем вариант который был правильным ответом то меняем статус на "Не выбрано"
                if (optionsLB.SelectedItem.ToString() == correctAnswerTBl.Text)
                    correctAnswerTBl.Text = unselectedText;
                // Если нет то просто удаляем и отключает кнопки
                optionsLB.Items.Remove(optionsLB.SelectedItem);
                turnOffButtons();
            }
        }

        private void correctAnswerBut_Click(object sender, RoutedEventArgs e)
        {
            // Меняем правильный ответ
            correctAnswerTBl.Text = optionsLB.SelectedItem.ToString();
        }

        private void addQuestionBut_Click(object sender, RoutedEventArgs e)
        {
            // Если все поля заполнены то добавляем вопрос
            if (questionTBox.Text != "" && optionsLB.Items != null && correctAnswerTBl.Text != unselectedText)
            {
                Questions question = new Questions()
                {
                    Question = questionTBox.Text,
                    Answer_id = null,
                    Test_id = MainClass.addedTestID

                };
                MainClass.db.Questions.Add(question);
                MainClass.db.SaveChanges();

                // Добавляем варианты ответа в соответствующую таблицу
                foreach (var item in optionsLB.Items)
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
                    // Очищаем все поля и отключаем кнопки
                    optionsLB.Items.Clear();
                    questionTBox.Text = "";
                    correctAnswerTBl.Text = unselectedText;
                    optionNameTBox.Text = "";
                    turnOffButtons();
                }
                else
                    MainClass.FrameVar.Navigate(new TeachersMenuPage());  // Возвращаемся в меню

            }
            else
                MessageBox.Show("Заполните все поля", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);

        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            testNameTBl.Text = MainClass.addedTestName; // Выводим имя теста к которому добавляем вопросы
        }

        private void exitToMenuBut_Click(object sender, RoutedEventArgs e)
        {
            MainClass.FrameVar.Navigate(new TeachersMenuPage());
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

        private void optionsLB_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            turnOnButtons();
        }
    }
}
