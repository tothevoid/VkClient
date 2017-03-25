using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace VkClient
{
    public class MyMessageBox
    {
        private static Window Box = new Window();

        static MyMessageBox()
        {
            MainGrid.Children.Add(DescriptionLb);
            MainGrid.Children.Add(BtnOk);
        }

        private static void Method()
        {
            Box = new Window()
            {
                Foreground = Brushes.White,
                Background = Brushes.Black,
                Height = 200,
                Width = 400,
                ResizeMode = ResizeMode.NoResize,
                ShowInTaskbar = true,
                WindowStartupLocation = WindowStartupLocation.CenterScreen
            };
            BtnOk.Click += BtnOk_Click;
            Box.Content = MainGrid;
        }


        private static Grid MainGrid = new Grid();

        private static TextBlock DescriptionLb = new TextBlock()
        {
            Margin = new Thickness(20, 10, 20, 10),
            Foreground = Brushes.White,
            TextWrapping = TextWrapping.Wrap
        };

        private static Button BtnOk = new Button()
        {
            Foreground = Brushes.White,
            Background = Brushes.Black,
            Content = "OK",
            Height = 20,
            Width = 50,
            HorizontalAlignment = HorizontalAlignment.Center,
            VerticalAlignment = VerticalAlignment.Bottom,
            Margin = new Thickness(0, 0, 0, 15),
        };

        private static void BtnOk_Click(object sender, RoutedEventArgs e)
        {
            Box.Close();
        }

        public static void Show(string Description)
        {
            Method();
            Box.Title = Description;
            Box.Show();
        }

        public static void Show(string Description, string Header)
        {
            Method();
            Box.Title = Description;
            DescriptionLb.Text = Header;
            Box.ShowDialog();

        }
    }
}
