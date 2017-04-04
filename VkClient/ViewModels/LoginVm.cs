using System;
using System.Net;
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
            bool result = CheckConnection("http://vk.com/");
            if (result == false)
            {
                CustomMessageBox.Show("Connection error", "You are currently offline");
                return;
            }
            var passbox = parameter as PasswordBox;
            if (Login == null || passbox.Password == null)
            {
                CustomMessageBox.Show("Login error", "One of field is empty");
                return;
            }
            try
            {
                api.Authorize(new ApiAuthParams // trying to auth
                {
                    ApplicationId = 5865343,
                    Login = Login,
                    Password = passbox.Password,
                    Settings = Settings.All,
                });
                api.Stats.TrackVisitor(); 
                Logged?.Invoke();
                api.Account.SetOffline();
                Opened = Visibility.Collapsed;
           
            }
            catch  
            {
               CustomMessageBox.Show("Login error","Invalid login or password"); //got exeption => notifying users
            
            }
        }

      

        private bool CheckConnection(String URL)
        {
            try
            {
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(URL);
                request.Timeout = 5000;
                request.Credentials = CredentialCache.DefaultNetworkCredentials;
                HttpWebResponse response = (HttpWebResponse)request.GetResponse();

                if (response.StatusCode == HttpStatusCode.OK)
                   return true;
                else
                   return false;
            }
            catch
            {
                return false;
            }
        }

    }

    
}
