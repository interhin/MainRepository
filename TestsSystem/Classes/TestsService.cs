using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestsSystem.Models;

namespace TestsSystem
{
    public static class TestsService
    {
        public static Tests addedTest { get; set; } // Тест который был добавлен пользователем
        public static Tests editingTest { get; set; } // Тест который редактируется
        public static Tests testingTest { get; set; } // Тест который проходится
        public static bool isEditingTest { get; set; } = false; // Редактируется ли сейчас тест
        public static bool isImportingTest { get; set; }
    }
}
