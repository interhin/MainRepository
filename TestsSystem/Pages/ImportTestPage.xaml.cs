using Microsoft.Office.Interop.Word;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using TestsSystem.Classes;
using TestsSystem.Models;


namespace TestsSystem.Pages
{
    /// <summary>
    /// Interaction logic for ImportTestPage.xaml
    /// </summary>
    public partial class ImportTestPage : System.Windows.Controls.Page
    {
        Microsoft.Office.Interop.Word.Application word;
        Document doc;

        public ImportTestPage()
        {
            InitializeComponent();
        }

        private async void selectFileBut_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Filter = "Word document(*.docx)|*.docx";
            if (ofd.ShowDialog() == true)
            {
                
                var extension = Path.GetExtension(ofd.FileName);
                if (extension == ".txt")
                {
                    // ...
                }
                else if (extension == ".docx")
                {
                    spinnerIcon.Visibility = Visibility.Visible;
                    var wordTest = await System.Threading.Tasks.Task.Run(()=> { return LoadWordDocument(ofd.FileName); });
                    spinnerIcon.Spin = false;
                    var newTest = await System.Threading.Tasks.Task.Run(() => { return InsertWordTestToDB(wordTest); });
                    OpenEditForm(newTest);
                   
                }
            }
        }

        private WordTest LoadWordDocument(string openDialogFilePath)
        {
            word = new Microsoft.Office.Interop.Word.Application();
            doc = new Document();

            object fileName = openDialogFilePath;
            // Пустой объект для ненужных параметров
            object missing = System.Type.Missing;

            try
            {

                // Открытие документа
                doc = word.Documents.Open(ref fileName,
                        ref missing, ref missing, ref missing, ref missing,
                        ref missing, ref missing, ref missing, ref missing,
                        ref missing, ref missing, ref missing, ref missing,
                        ref missing, ref missing, ref missing);
            }
            catch (Exception ex)
            {
                MessageService.ShowError("Ошибка при загрузке документа: " + ex.Message);
                this.Dispatcher.Invoke(() => { spinnerIcon.Visibility = Visibility.Hidden; });
                return null;
            }
            WordTest wordTest = new WordTest(); // Создаем тест
            wordTest.TestName = doc.Paragraphs[1].Range.Text.Replace("\r", ""); // Первый параграф - имя теста

            // Парсим время на прохождение теста из второго параграфа
            Regex timeRegular = new Regex("[0-9]{2}:[0-9]{2}:[0-9]{2}");
            string[] wordTime = timeRegular.Match(doc.Paragraphs[2].Range.Text).Value.Split(':');


            int testTimeInSeconds;
            // Если получилось спарсить то грузим вопрос и варианты ответа
            if (wordTime.Length != 0)
            {
                // Переводим время в секунды
                testTimeInSeconds = CalcService.HoursToSeconds(int.Parse(wordTime[0])) +
                                    CalcService.MinutesToSeconds(int.Parse(wordTime[1])) +
                                    int.Parse(wordTime[2]);
                wordTest.TestPassTime = testTimeInSeconds;

                wordTest.TestQuestions = new ObservableCollection<WordQuestions>();
                for (int i = 1; i <= doc.Lists[1].ListParagraphs.Count; i++)
                {
                    WordQuestions question = new WordQuestions() { QuestionText = doc.Lists[1].ListParagraphs[i].Range.Text.Replace("\r", "") };
                    question.OptionsList = new ObservableCollection<string>(doc.Lists[i + 1].Range.Text.Split('\r').Where(x => !String.IsNullOrWhiteSpace(x)).ToList());
                    wordTest.TestQuestions.Add(question);
                }
            }

            ((_Document)doc).Close();
            ((_Application)word).Quit();
            return wordTest;
        }

        private Tests InsertWordTestToDB(WordTest wordTest)
        {
            var numQuestions = wordTest.TestQuestions.Count;

            var newTest = new Tests()
            {
                AuthorID = UsersService.currentUser.id,
                Name = wordTest.TestName,
                NumToPassFive = numQuestions,
                NumToPassFour = (int)Math.Round(numQuestions * 0.75),
                NumToPassThree = (int)Math.Round(numQuestions * 0.5,MidpointRounding.AwayFromZero),
                PassTime = wordTest.TestPassTime,
                QuestionTime = 0
            };
            MainClass.db.Tests.Add(newTest);
            MainClass.db.SaveChanges();
            foreach (var q in wordTest.TestQuestions)
            {
                var newQuestion = new Questions()
                {
                    Text = q.QuestionText,
                    TestID = newTest.id
                };
                MainClass.db.Questions.Add(newQuestion);
                MainClass.db.SaveChanges();

                foreach (var op in q.OptionsList)
                {
                    if (!op.Contains(" (+)"))
                    {
                        MainClass.db.Options.Add(new Models.Options()
                        {
                            QuestionID = newQuestion.id,
                            Text = op,
                        });
                    }
                    else
                    {
                        var newAnswer = new Models.Options()
                        {
                            Text = op.Replace(" (+)", ""),
                            QuestionID = newQuestion.id
                        };
                        MainClass.db.Options.Add(newAnswer);
                        MainClass.db.SaveChanges();
                        newQuestion.AnswerID = newAnswer.id;
                        MainClass.db.SaveChanges();
                    }
                }
            }

            return newTest;
        }

        private void OpenEditForm(Tests newTest)
        {
            if (MessageService.ShowYesNoInfo("Тест успешно загружен в базу! Желаете изменить данные теста?") == MessageBoxResult.Yes)
            {
                TestsService.isEditingTest = true;
                TestsService.editingTest = newTest;
                TestsService.isImportingTest = true;
                MainClass.FrameVar.Navigate(new CreateEditTestPage());
            }
            else
            {
                MainClass.FrameVar.Navigate(new TeachersMenuPage());
            }
        }
    }
}
