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
    public partial class AuthPage : Page
    {
        public AuthPage()
        {
            InitializeComponent();
        }

        private void AuthBut_Click(object sender, RoutedEventArgs e)
        {
            MainClass.disableBack = false;
            Users user = new Users();
            // Проверяем введенные данные
            if (CheckUser(loginTBox.Text,passTBox.Password,ref user))
            {
                // Если вошли то добавляем пользователя в публичный класс чтобы запомнить какой пользователь вошёл
                CurrentUser.curUser = user;
                // Проверяем кто вошел (0 - Админ, 1 - Учитель, 2 - Студент) 
                switch (user.Role) {
                    case 0:
                        MainClass.FrameVar.Navigate(new AdminsMenuPage());
                        break;
                    case 1:
                        MainClass.FrameVar.Navigate(new TeachersMenuPage());
                        break;
                    case 2:
                        MainClass.FrameVar.Navigate(new StudentsMenuPage());
                        break;
                }
            }
            else
            {
                MessageBox.Show("Неверный логин или пароль!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        bool CheckUser(String Login, String Password, ref Users user)
        {
            user = MainClass.db.Users.Where(x => x.Login == loginTBox.Text && x.Password == passTBox.Password).FirstOrDefault();

            if (user == null)
                return false;

            return true;
        }
    }
}
