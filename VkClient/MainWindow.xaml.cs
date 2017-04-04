using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;
using System.Windows.Media;
using System.Windows.Input;
using System.Collections.Generic;

namespace VkClient
{
    public partial class MainWindow : Window
    {
        private static Window MainWindowInstance;

        //Getting instances of ViewModels
        FriendsVm FrVm = new FriendsVm();
        LoginVm LgVm = new LoginVm();
        SavedPhotoVm SfVm = new SavedPhotoVm();

        public MainWindow()
        {
            InitializeComponent();

            MainWindowInstance = this;

            //Connecting ViewModels to tabs
            FriendsTab.DataContext = FrVm;
            LoginTab.DataContext = LgVm;
            SavedPhotosTab.DataContext = SfVm;

            //Loading friends after login
            LgVm.Logged += FrVm.Get_friends;
            LgVm.Logged += SfVm.SwitchVisibility;
            LgVm.Logged += GetFocus;
        }

        private void GetFocus()
        {
            SavedPhotosTab.Focus();
        }

        public static void SetOwned(ref Window Owned)
        {
            Owned.Owner = MainWindowInstance;
        }

        private void KeyPushed(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Left)
                SfVm.Prev(null);
            else if (e.Key == Key.Right)
                SfVm.Next(null);
            else if (e.Key == Key.F5)
                SfVm.Update();
        }


    }

}
