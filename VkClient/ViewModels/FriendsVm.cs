using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using VkNet.Enums.Filters;
using VkNet.Model.RequestParams;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace VkClient
{
    class FriendsVm : VmBase
    {
        public ObservableCollection<Friend> Friends { get; set; } = new ObservableCollection<Friend>();
        public ObservableCollection<Message> MyMessages { get; set; } = new ObservableCollection<Message>();

        private int _totalFriends;
        private int _onlineFriends;
        private int _column;
        private long? ChatId;

        private Friend _selectedFriend;

        public Friend SelectedFriend
        {
            get { return _selectedFriend; }
            set { Set(ref _selectedFriend, value); GetMessages(value); }
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

        public int Column
        {
            get { return _column; }
            set { Set(ref _column, value); }
        }

        public void Get_friends() // Getting friend list
        {
            int Count = 0;
            var query = new FriendsGetParams { UserId = api.UserId, Fields = ProfileFields.All }; // Getting needed fields  
            var users = api.Friends.Get(query).OrderByDescending(x => x.Online); // Friends sort
            foreach (var x in users)
            {
                SolidColorBrush status;
                if (x.Online == true) // Setting status color
                    status = x.OnlineMobile == true
                        ? new SolidColorBrush(Colors.CornflowerBlue)
                        : new SolidColorBrush(Colors.DarkGreen);
                else
                {
                    status = new SolidColorBrush(Colors.Red);
                    Count++;
                }
                Friend fr = new Friend(x.FirstName, x.LastName, new BitmapImage(x.Photo50), status, x.Id); // Get instances of friend's class
                Friends.Add(fr); //adding it to list of all friends
            }
            TotalFriends = Friends.Count;
            OnlineFriends = Friends.Count - Count;
           
        }

        private void GetMessages(Friend friend)
        {
            MyMessages.Clear();
            var Dialogs = api.Messages.GetDialogs(new MessagesDialogsGetParams{Count = 1});
            
            foreach (var x in Dialogs.Messages)
            {
                ChatId = x.ChatId;
            }

            var DialogMessages = api.Messages.GetHistory(new MessagesGetHistoryParams
            {
                Count = 200,
                UserId = friend.Id
                //UserId = ChatId + 2000000000, //Chat id
            });
            ConvertToMessage(DialogMessages);
        }

        private void ConvertToMessage(VkNet.Model.MessagesGetObject collection)
        {
            List<Message> objs = new List<Message>();
            foreach (var x in collection.Messages)
            {
                if (x.Attachments.Count == 0 || x.Attachments[0].Type.Name != "Photo")
                    continue;
                string text = x.Body;
                var photo = x.Attachments[0].Instance as VkNet.Model.Attachments.Photo;
                var message = new Message(text, new BitmapImage(photo.Photo604), x.Type);
                MyMessages.Add(message);
            }
          
        }
    }
}
