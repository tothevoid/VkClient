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
        public delegate void Get_friends();

        public event Get_friends Logged;

        private string _login;

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
            }
            catch
            {
                MessageBox.Show("Try again", "Login exeption"); //got exeption => notifying user
            }
        }




    }
}
