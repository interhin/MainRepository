using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace TestsSystem
{
    public static class MessageService
    {
        public static void ShowError(string errorText)
        {
            MessageBox.Show(errorText, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
        }

        public static void ShowInfo(string infoText)
        {
            MessageBox.Show(infoText, "Информация", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        public static void ShowWarning(string warningText)
        {
            MessageBox.Show(warningText, "Внимание", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        public static MessageBoxResult ShowYesNoError(string errorText)
        {
            return MessageBox.Show(errorText, "Ошибка", MessageBoxButton.YesNo, MessageBoxImage.Error);
        }

        public static MessageBoxResult ShowYesNoInfo(string infoText)
        {
            return MessageBox.Show(infoText, "Информация", MessageBoxButton.YesNo, MessageBoxImage.Information);
        }

        public static MessageBoxResult ShowYesNoWarning(string warningText)
        {
            return MessageBox.Show(warningText, "Внимание", MessageBoxButton.YesNo, MessageBoxImage.Information);
        }
    }
}
