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
    /// Interaction logic for AddUserPanel.xaml
    /// </summary>
    /// 
    public class Role
    {
        public int RoleID { get; set; }
        public string RoleName { get; set; }
    }

    public partial class AddUserPage : Page
    {
        public AddUserPage()
        {
            InitializeComponent();
        }

        private void addUserBut_Click(object sender, RoutedEventArgs e)
        {
            // Проверяем пустые ли поля
            if (!String.IsNullOrWhiteSpace(loginTBox.Text) &&
                !String.IsNullOrWhiteSpace(passTBox.Password) &&
                !String.IsNullOrWhiteSpace(nameTBox.Text) &&
                !String.IsNullOrWhiteSpace(surnameTBox.Text))
            {
                // Проверяем зарегистрирован ли уже пользователь с таким логином
                if (!UsersService.SameLogin(loginTBox.Text))
                {
                    Users user = new Users()
                    {
                        Login = loginTBox.Text,
                        Password = passTBox.Password,
                        Name = nameTBox.Text,
                        Surname = surnameTBox.Text,
                        Role = Convert.ToInt32(roleCB.SelectedValue)
                    };
                    MainClass.db.Users.Add(user);
                    MainClass.db.SaveChanges();
                    MessageService.ShowInfo("Пользователь успешно добавлен!");
                    MainClass.FrameVar.Navigate(new AdminsMenuPage());
                }
                else
                    MessageService.ShowError("Пользователь с таким логином уже существует!");
            }
            else
                MessageService.ShowError("Заполните все поля");
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            // Загрузка списка ролей
            List<Role> roles = new List<Role>();

            roles.Add(new Role() {RoleID = 0,RoleName = "Администратор" });
            roles.Add(new Role() { RoleID = 1, RoleName = "Учитель" });
            roles.Add(new Role() { RoleID = 2, RoleName = "Студент" });

            roleCB.DisplayMemberPath = "RoleName";
            roleCB.SelectedValuePath = "RoleID";

            roleCB.ItemsSource = roles;

            roleCB.SelectedIndex = 0;
        }
    }
}
