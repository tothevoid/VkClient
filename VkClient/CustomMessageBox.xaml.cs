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
using System.Windows.Shapes;

namespace VkClient
{
    /// <summary>
    /// Interaction logic for CustomMessageBox.xaml
    /// </summary>
    public partial class CustomMessageBox : Window
    {
        //TODO: hide basic methods
        //TODO: dynamic expand

        private static MessageBoxResult result = MessageBoxResult.No;
        private static Window _window;


        private CustomMessageBox()
        {
            InitializeComponent();
            Focus();
        }

        public static void Show(string Title)
        {
            _window = new CustomMessageBox { Title = Title };
            _window.ShowDialog();
        }
        
        public static void Show(string Title, string Description)
        {
            _window = new CustomMessageBox { Title = Title, DescriptionTb = { Text = Description } };
            MainWindow.SetOwned(ref  _window);     
            //_window.Owner = MainWindow.MainWindowInstance;
            _window.ShowDialog();
        }

        public static MessageBoxResult Show(string Title, string Description, MessageBoxType type)
        {
            _window = new CustomMessageBox { Title = Title, DescriptionTb = { Text = Description }, OkBtn = { Visibility = Visibility.Hidden }, NoBtn = {Visibility=Visibility.Visible }, YesBtn = {Visibility = Visibility.Visible } };
            MainWindow.SetOwned(ref _window);
            _window.ShowDialog();
            return result;
        }

        private void BaseClick(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void YesClick(object sender, RoutedEventArgs e)
        {
            result = MessageBoxResult.Yes;
            Close();
        }
    }

    public enum MessageBoxType
    {
        Ok = 0,
        YesNo = 1
    }




}
