using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using VkNet;
using VkNet.Enums;
using VkNet.Enums.Filters;
using VkNet.Enums.SafetyEnums;
using VkNet.Model;
using VkNet.Model.RequestParams;
using VkNet.Utils.AntiCaptcha;
using Button = System.Windows.Controls.Button;

namespace VkClient
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        // int avatar_index = 0;
        // long friend_id;

        /*
        List<Friend> my_friends = new List<Friend>(); // list of friends
        List<SavedPhoto> pics = new List<SavedPhoto>();
        List<Uri> avatars = new List<Uri>();
       */

        public MainWindow()
        {
            InitializeComponent();

            //Getting instances of ViewModels
            var FrVm = new FriendsVm();
            var LgVm = new LoginVm();
            var SfVm = new SavedPhotoVm();

            //Connecting ViewModels to tabs
            FriendsTab.DataContext = FrVm;
            LoginTab.DataContext = LgVm;
            SavedPhotosTab.DataContext = SfVm;

            //Loading friends after login
            LgVm.Logged += FrVm.Get_friends;
        }

        private void Start_click(object sender, RoutedEventArgs e)
        {
            /* MESSAGE IMPLEMENTION
            //UNODNE  message loading implemention
            var a =
                api.Messages.Get(new MessagesGetParams
                {
                    Count = 200,
                    Filters = MessagesFilter.All,
                    PreviewLength = 0,
                    Out = MessageType.Received
                });
            var b =
                api.Messages.Get(new MessagesGetParams
                {
                    Count = 200,
                    Filters = MessagesFilter.All,
                    PreviewLength = 0,
                    Out = MessageType.Sended
                });
            //var b = api.Messages.GetDialogs(new MessagesDialogsGetParams {Count = 200, PreviewLength = 0});
            List<Message> objs = new List<Message>();
            foreach (var x in a.Messages)
            {
                string f1 = x.Body;
                BitmapImage f2;
                if (x.Attachments.Count == 0)
                    continue;
                if (x.Attachments[0].Type.Name != "Photo")
                    continue;
                var c = x.Attachments[0].Instance as VkNet.Model.Attachments.Photo;
                f2 = new BitmapImage(c.Photo604);
                var ef = new Message(f1, f2, null, null);
                objs.Add(ef);
            }
            //  Chat.ItemsSource = objs;
             string txt = Clipboard.GetText();
             */
        }


        /*
             private void more_click(object sender, RoutedEventArgs e) //loading more saved photos
             {
                 Get_saved_photos(Convert.ToInt32(user_id.Text));
             }

             private void keyboard_inputs(object sender, KeyEventArgs e)  // keyboard inputs
             {
                 if (e.Key == Key.Left)
                     Check(false);
                 if (e.Key == Key.Right)
                     Check(true);
                 if (e.Key == Key.Escape)
                     Close();
                 if (e.Key == Key.Enter)
                     more_click(null, null);
             }

             private void Switch_user_click(object sender, RoutedEventArgs e)
             {
                 _offset = 0;
                 Get_saved_photos(Convert.ToInt32(user_id.Text));
             }

             private void Lw_click(object sender, SelectionChangedEventArgs e)
             {
                 var LV = sender as ListView;
                 var Fr = LV.SelectedItem as Friend;
                 friend_id = Fr.Id;
                 Got_avatars();
             }

             private void Got_avatars() 
             {
                 avatar_index = 0;
                 avatars.Clear();
                 var details = new VkNet.Model.RequestParams.PhotoGetParams { Extended = true, AlbumId = VkNet.Enums.SafetyEnums.PhotoAlbumType.Profile, Count = 50, Reversed = true, OwnerId = friend_id, Offset = _offset };
                 Load_profile(friend_id);
                 var collection = api.Photo.Get(details); // TODO: Fix exeption with opening of deleted profile
                 foreach (var x in collection)
                     avatars.Add(x.Photo604);
                 if (avatars.Count != 0)
                     Fr_img.Source = new BitmapImage(avatars[0]);
             }



             private void Next_click(object sender, RoutedEventArgs e) //next avatar button event
             {
                 var determine = sender as Button;
                 if (Fr_img.Source == null || avatars.Count == 0)  
                     return;
                 switch (determine.Content.ToString())
                 {
                     case ("Prev"):
                         if (avatar_index - 1 <0)
                             return;
                         avatar_index--;
                         Fr_img.Source = new BitmapImage(avatars[avatar_index]);
                         break;
                     case ("Next"):
                         if (avatar_index + 1 > avatars.Count - 1)
                             return;
                         avatar_index++;
                         Fr_img.Source = new BitmapImage(avatars[avatar_index]);
                         break;
                 }
             }

             private void Load_profile(long Id) // UNDONE: profiles
             {
                 var profile = api.Users.Get(Id, ProfileFields.All);
                 User_avatar.Source = new BitmapImage(profile.Photo400Orig);
                 User_status.Text = profile.Status;
                 var Date = profile.BirthDate.Split('.'); // UNDONE: NO DATE
                 if (Date.Length == 3) // UNDONE: DD/MM date and DD/MM/YYYY date 
                 {
                     DateTime date = new DateTime(Convert.ToInt32(Date[2]), Convert.ToInt32(Date[1]), Convert.ToInt32(Date[0]));
                     User_birthday.Text = "Birthday:   "+date.ToString("D");
                 }
                 try
                 {
                     User_city.Text = "Current city:   "+profile.City.Title;
                 }
                 catch
                 {
                     User_city.Text = "Hidden";
                 }
                 User_name.Text = (profile.FirstName +" "+ profile.LastName);
                 Groups_count.Text = profile.Counters.Groups.ToString();
                 Photos_count.Text = profile.Counters.Photos.ToString();
                 Audios_count.Text = profile.Counters.Audios.ToString();
                 Videos_count.Text = profile.Counters.Videos.ToString();
                 User_online.Text = profile.Online==true ? "Online" : profile.LastSeen.Time.Value.ToString();
             }

             private void Search_query_changed(object sender, TextChangedEventArgs e)
             {
                 string curr_search = Search_query.Text;
                 Friends.ItemsSource = my_friends.Where(x => x.FirstName.ToLower().Contains(curr_search.ToLower()) || x.LastName.ToLower().Contains(curr_search.ToLower()));
             }

             private void Liked(object sender, RoutedEventArgs e)
             {
                 long photo_id = pics[curr_index].Id;
                 api.Likes.Add(new LikesAddParams {ItemId = photo_id, Type = LikeObjectType.Photo});


             }
             */

            // TODO: Album save
            // TODO: Img cache
            // TODO: Groups 
            // TODO: Messages
            // TODO: Video
            // TODO: Feed
            // TODO: Img cache
    }
}
