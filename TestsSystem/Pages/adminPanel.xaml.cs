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
    /// Interaction logic for adminPanel.xaml
    /// </summary>
    public partial class adminPanel : Page
    {
        public adminPanel()
        {
            InitializeComponent();
        }

        private void addUserBut_Click(object sender, RoutedEventArgs e)
        {
            MainClass.FrameVar.Navigate(new AddUserPanel());
        }

        private void delUserBut_Click(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show("Вы уверены что хотите удалить эту запись?", "Внимание", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.Yes)
            {
                if ((usersGrid.SelectedItem as Users)!=null)
                {
                    MainClass.db.Users.Remove((usersGrid.SelectedItem as Users));
                    MainClass.db.SaveChanges();
                    usersGrid.ItemsSource = MainClass.db.Users.ToList();
                }
            }
        }

        private void saveBut_Click(object sender, RoutedEventArgs e)
        {
            MainClass.db.SaveChanges();
            MessageBox.Show("Изменения успешно сохранены","Информация",MessageBoxButton.OK,MessageBoxImage.Information);
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            usersGrid.ItemsSource = MainClass.db.Users.ToList();
        }
    }
}
