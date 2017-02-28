using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using VkNet.Enums.Filters;
using VkNet.Model.RequestParams;

namespace VkClient
{

    class FriendsVm : VmBase
    {
        public ObservableCollection<Friend> Friends { get; set; } = new ObservableCollection<Friend>();

        private  int _totalFriends;
        private int _onlineFriends;

        public ICommand Reload => new CommandBase(Update);

        private void Update(object parameter)
        {
            Friends.Clear();
            Get_friends();
        } 
        
        public int TotalFriends
        {
            get { return _totalFriends; }
            set { Set(ref _totalFriends, value); }
        }

        public int OnlineFriends
        {
            get { return _onlineFriends; }
            set { Set(ref _onlineFriends, value); }
        }

        public void Get_friends() //Loading friendlist
        {
            int Count=0;
            var query = new FriendsGetParams { UserId = api.UserId, Fields = ProfileFields.All }; // getting needed fields  
            var users = api.Friends.Get(query).OrderByDescending(x => x.Online); //starting to get friends
            foreach (var x in users)
            {
                SolidColorBrush status;
                if (x.Online == true) // setting online status color
                    status = x.OnlineMobile == true
                        ? new SolidColorBrush(Colors.CornflowerBlue)
                        : new SolidColorBrush(Colors.DarkGreen);
                else
                {
                    status = new SolidColorBrush(Colors.Red);
                    Count++;
                }
                Friend fr = new Friend(x.FirstName, x.LastName, new BitmapImage(x.Photo50), status, x.Id); // get instances of friend's class
                Friends.Add(fr); //adding it to list of all friends
            }
            TotalFriends = Friends.Count;
            OnlineFriends = Friends.Count - Count;
        }
    }
}
