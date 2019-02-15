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
        public ImportTestPage()
        {
            InitializeComponent();
        }

        private void selectFileBut_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Filter = "Word document(*.docx)|*.docx|Text files(*.txt)|*.txt";
            if (ofd.ShowDialog() == true)
            {
                var extension = Path.GetExtension(ofd.FileName);
                if (extension == ".txt")
                {

                }
                else if (extension == ".docx")
                {
                    Microsoft.Office.Interop.Word.Application word = new Microsoft.Office.Interop.Word.Application();
                    Document doc = new Document();

                    object fileName = ofd.FileName;
                    // Пустой объект для бесполезных параметров
                    object missing = System.Type.Missing;
                    // Открытие документа
                    doc = word.Documents.Open(ref fileName,
                            ref missing, ref missing, ref missing, ref missing,
                            ref missing, ref missing, ref missing, ref missing,
                            ref missing, ref missing, ref missing, ref missing,
                            ref missing, ref missing, ref missing);

                    WordTest wordTest = new WordTest(); // Создаем тест
                    wordTest.TestName = testNameTBox.Text = doc.Paragraphs[1].Range.Text.Replace("\r",""); // Первый параграф - имя теста

                    // Парсим время на прохождение теста из второго параграфа
                    Regex timeRegular = new Regex("[0-9]{2}:[0-9]{2}:[0-9]{2}");
                    string[] wordTime = timeRegular.Match(doc.Paragraphs[2].Range.Text).Value.Split(':');


                    int testTimeInSeconds;
                    // Если получилось спарсить то грузим вопрос и варианты ответа
                    if (wordTime.Length != 0)
                    {
                        passTimeTBox.Text = timeRegular.Match(doc.Paragraphs[2].Range.Text).Value;
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
                        questionsLB.ItemsSource = wordTest.TestQuestions;
                    }
                    ((_Document)doc).Close();
                    ((_Application)word).Quit();

                }
            }
        }

        private void questionsLB_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            optionsLB.ItemsSource = (questionsLB.SelectedItem as WordQuestions).OptionsList;
        }
    }
}
