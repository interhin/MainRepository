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
    /// Interaction logic for Auth.xaml
    /// </summary>
    public partial class Auth : Page
    {
        public Auth()
        {
            InitializeComponent();
        }

        private void AuthBut_Click(object sender, RoutedEventArgs e)
        {
            // Ищем пользователя в БД (Вернет null если такого пользователя нет)
            var user = MainClass.db.Users.Where(x => x.Login == login.Text && x.Password == pass.Password).FirstOrDefault();
            if (user != null)
            {
                // Если вошли то добавляем пользователя в публичный класс чтобы запомнить какой пользователь вошёл
                CurrentUser.curUser = user;
                // Проверяем кто вошел (0 - Админ, 1 - Учитель, 2 - Студент) 
                if (user.Role == 0)
                {
                    MainClass.FrameVar.Navigate(new adminPanel());
                } else if (user.Role == 1)
                {
                    MainClass.FrameVar.Navigate(new MenuPanel());
                } else if (user.Role == 2)
                {
                    MainClass.FrameVar.Navigate(new StudentsPanel());
                }
            }
            else
            {
                MessageBox.Show("Неверный логин или пароль!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
