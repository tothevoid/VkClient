using System;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows;
using System.Windows.Controls;

namespace VkClient
{
    class Friend
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public SolidColorBrush Status { get; set;}
        public BitmapImage Ref { get; set; }
        public long Id;

        public Friend(string Fn,string Ln, BitmapImage Ref, SolidColorBrush Status, long Id)
        {
            FirstName = Fn;
            LastName = Ln;
            this.Ref = Ref;
            this.Status = Status;
            this.Id = Id;
        }
    }

    class SavedPhoto
    {
        public Uri Link {get;set;}
        public DateTime Date { get; set; }
        public int Likes { get; set; }
        public long Id { get; set; }
        public bool IsLiked { get; set; }

        public SavedPhoto(Uri link, DateTime date,int likes,long id,bool il)
        {
            Link = link;
            Date = date;
            Likes = likes;
            Id = id;
            IsLiked = il;
        }

    }

    class Message
    {
        public string Text { get; set; }
        public BitmapImage Image { get; set; }
     //   public int Column { get; set; }
        public  HorizontalAlignment Alignment {get;set;}
        public Dock Dock { get; set; }

        public Message(string msg, BitmapImage photo, VkNet.Enums.MessageType? type)
        {
            
            if (type == null)
                throw new NullReferenceException();
            Text = msg;
            Image = photo;
            //if (type == VkNet.Enums.MessageType.Received)
            //    Alignment = HorizontalAlignment.Left;
            //else
            //    Alignment = HorizontalAlignment.Right; 

            if (type == VkNet.Enums.MessageType.Received)
                Dock = Dock.Left;
            else
                Dock = Dock.Right;
            
            
            //Column = (int) type.Value;
        }
    }

}
