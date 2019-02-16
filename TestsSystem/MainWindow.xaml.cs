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

namespace TestsSystem
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void backBut_Click(object sender, RoutedEventArgs e)
        {
            frame.GoBack();
        }

        private void frame_Navigated(object sender, NavigationEventArgs e)
        {
            backBut.IsEnabled = MainClass.disableBack == true ? false : frame.CanGoBack;
            homeBut.IsEnabled = !(frame.Content is Pages.AuthPage);

            if (!(frame.Content is Pages.AuthPage)
                && !(frame.Content is Pages.TeachersMenuPage)
                && !(frame.Content is Pages.StudentsMenuPage)
                && !(frame.Content is Pages.AdminsMenuPage))
            {
                backToMenuBut.IsEnabled = true;
            }
            else
            {
                backToMenuBut.IsEnabled = false;
            }

        }

        private void homeBut_Click(object sender, RoutedEventArgs e)
        {
            while (frame.CanGoBack)
                frame.GoBack();
        }

        private void Window_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            CenteringWindow(e.PreviousSize, e.NewSize); // Центрируем окно если размеры изменились
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            MainClass.FrameVar = frame; // Передаем экземпляр Frame'a в public класс чтобы к нему был доступ всем
            frame.Navigate(new Pages.AuthPage());
        }

        void CenteringWindow(Size PreviousSize, Size NewSize)
        {
            if (PreviousSize == NewSize)
                return;

            var w = SystemParameters.PrimaryScreenWidth;
            var h = SystemParameters.PrimaryScreenHeight;

            this.Left = (w - NewSize.Width) / 2;
            this.Top = (h - NewSize.Height) / 2;
        }

        private void backToMenuBut_Click(object sender, RoutedEventArgs e)
        {
            switch (UsersService.currentUser.RoleID)
            {
                case 1:
                    frame.Navigate(new Pages.AdminsMenuPage());
                    break;
                case 2:
                    frame.Navigate(new Pages.TeachersMenuPage());
                    break;
                case 3:
                    frame.Navigate(new Pages.StudentsMenuPage());
                    break;
            }
        }
    }
}
