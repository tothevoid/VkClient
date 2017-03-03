using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using VkNet;
using VkNet.Enums.Filters;

namespace VkClient
{

    class LoginVm : VmBase
    {
        public delegate void GetFriends();

        public event GetFriends Logged;

        private string _login;

        private Visibility _opened = Visibility.Visible;

        public Visibility Opened
        {
            get { return _opened; }
            set { Set(ref _opened, value); }
        }


        public string Login
        {
            get { return _login; }
            set { Set(ref _login, value); }
        }

        public ICommand Auth => new CommandBase(TryToAuth);

        private void TryToAuth(object parameter)
        {
            var passbox = parameter as PasswordBox;
            try
            {
                api.Authorize(new ApiAuthParams //trying to auth
                {
                    ApplicationId = 5865343,
                    Login = Login,
                    Password = passbox.Password,
                    Settings = Settings.All,
                });
                if (Logged != null)
                    Logged();
                Opened = Visibility.Collapsed;
            }
            catch
            {
                MessageBox.Show("Try again", "Login exeption"); //got exeption => notifying user
            }
        }




    }
}
