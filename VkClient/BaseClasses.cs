﻿using System;
using System.Windows.Media;
using System.Windows.Media.Imaging;


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
        public string Date { get; set; }
        public int Likes { get; set; }
        public long Id { get; set; }
        public bool IsLiked { get; set; }

        public SavedPhoto(Uri link,string date,int likes,long id,bool il)
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
        public string Recieved_msg { get; set; }
        public BitmapImage Recieved_img { get; set; }
        public string Sent_msg { get; set; }
        public BitmapImage Sent_img { get; set; }

        public Message(string rm,BitmapImage ri, string sm,BitmapImage si)
        {
            Recieved_msg = rm;
            Recieved_img = ri;
            Sent_msg = sm;
            Sent_img = si;
        }
}
}
