using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestsSystem.Classes
{
    public class WordQuestions
    {
        public string QuestionText { get; set; }
        public ObservableCollection<string> OptionsList { get; set; }
    }
}
