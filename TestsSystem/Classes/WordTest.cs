using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestsSystem.Classes
{
    public class WordTest
    {
        public string TestName { get; set; }
        public int TestPassTime { get; set; }
        public int TestQuestionTime { get; set; }
        public int NumToFive { get; set; }
        public int NumToFour { get; set; }
        public int NumToThree { get; set; }
        public ObservableCollection<WordQuestions> TestQuestions { get; set; }
    }
}
