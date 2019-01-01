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
using TestsSystem.Pages;

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
            backBut.IsEnabled = frame.CanGoBack;
            homeBut.IsEnabled = !(frame.Content is Pages.AuthPage);

        }

        private void homeBut_Click(object sender, RoutedEventArgs e)
        {
            frame.Navigate(new AuthPage());
        }

        private void Window_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            if (e.PreviousSize == e.NewSize)
                return;

            var w = SystemParameters.PrimaryScreenWidth;
            var h = SystemParameters.PrimaryScreenHeight;

            this.Left = (w - e.NewSize.Width) / 2;
            this.Top = (h - e.NewSize.Height) / 2;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            MainClass.FrameVar = frame;
            MainClass.FrameVar.Navigate(new AuthPage());
        }
    }
}
